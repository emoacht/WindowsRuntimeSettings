using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage;

namespace WindowsRuntimeSettings.Accessor
{
	/// <summary>
	/// Save/Load settings in an encrypted file in LocalFolder/RoamingFolder.
	/// </summary>
	internal class CryptFileRootAccessor : IRootAccessor
	{
		#region Constant

		/// <summary>
		/// Primitive types supported by <see cref="DataContractSerializer"/> by default
		/// </summary>
		/// <remarks>
		/// https://msdn.microsoft.com/en-us/library/ms731923.aspx
		/// </remarks>
		private static readonly Type[] _primitiveTypes = new[]
		{
			typeof(byte),
			typeof(sbyte),
			typeof(short),
			typeof(int),
			typeof(long),
			typeof(ushort),
			typeof(uint),
			typeof(ulong),
			typeof(float),
			typeof(double),
			typeof(bool),
			typeof(char),
			typeof(decimal),
			typeof(object),
			typeof(string),
			typeof(DateTime),
			typeof(TimeSpan),
			typeof(Guid),
			typeof(Uri),
			typeof(byte[])
		};

		#endregion

		private readonly string _cryptFileName;
		private readonly ReaderWriterLockSlim _locker = new ReaderWriterLockSlim(); // IDisposable

		public CryptFileRootAccessor(string cryptFileName)
		{
			if (string.IsNullOrWhiteSpace(cryptFileName))
				throw new ArgumentNullException(nameof(cryptFileName));

			_cryptFileName = cryptFileName;
		}

		#region Getter

		public T GetValue<T>(string propertyName, bool isRoaming)
		{
			T propertyValue;
			TryGetValue(out propertyValue, propertyName, isRoaming);
			return propertyValue;
		}

		public T GetValue<T>(T defaultValue, string propertyName, bool isRoaming)
		{
			T propertyValue;
			if (TryGetValue(out propertyValue, propertyName, isRoaming))
				return propertyValue;
			else
				return defaultValue;
		}

		public bool TryGetValue<T>(out T propertyValue, string propertyName, bool isRoaming)
		{
			var folder = GetStorageFolder(isRoaming);

			_locker.EnterReadLock();
			try
			{
				var pairs = Task.Run(() => LoadAsync(folder)).Result; // Task.Run is to avoid a deadlock.
				if ((pairs != null) && pairs.ContainsKey(propertyName))
				{
					propertyValue = (T)pairs[propertyName];
					return true;
				}
				else
				{
					propertyValue = default(T);
					return false;
				}
			}
			finally
			{
				_locker.ExitReadLock();
			}
		}

		#endregion

		#region Setter

		public void SetValue<T>(T propertyValue, string propertyName, bool isRoaming)
		{
			var folder = GetStorageFolder(isRoaming);

			_locker.EnterWriteLock();
			try
			{
				var pairs = Task.Run(() => LoadAsync(folder)).Result; // Task.Run is to avoid a deadlock.		
				if (pairs != null)
				{
					if (pairs.ContainsKey(propertyName))
					{
						if (!EqualityComparer<T>.Default.Equals(propertyValue, default(T)))
						{
							pairs[propertyName] = propertyValue;
						}
						else
						{
							pairs.Remove(propertyName);
						}
					}
					else
					{
						if (!EqualityComparer<T>.Default.Equals(propertyValue, default(T)))
						{
							pairs.Add(propertyName, propertyValue);
						}
						else
						{
							// Go through for the case where the credential file exists but has no value.
						}
					}
				}
				else
				{
					if (!EqualityComparer<T>.Default.Equals(propertyValue, default(T)))
					{
						pairs = new Dictionary<string, object> { { propertyName, propertyValue } };
					}
					else
					{
						return;
					}
				}

				if (pairs.Any())
				{
					Task.Run(() => SaveAsync(pairs, folder)).Wait(); // Task.Run is to avoid a deadlock.
				}
				else
				{
					Task.Run(() => DeleteAsync(folder)).Wait(); // Task.Run is to avoid a deadlock.
				}
			}
			finally
			{
				_locker.ExitWriteLock();
			}
		}

		#endregion

		#region Access

		[DataContract]
		public class DataContainer
		{
			[DataMember]
			public string[] TypeNames { get; set; }

			[DataMember]
			public byte[] Data { get; set; }
		}

		private Type[] _knownTypes;

		private async Task SaveAsync(IDictionary<string, object> data, IStorageFolder folder)
		{
			_knownTypes = data
				.Select(x => x.Value.GetType())
				.Distinct()
				.Except(_primitiveTypes)
				.ToArray();

			try
			{
				byte[] serializedData;
				using (var stream = new MemoryStream())
				{
					var serializer = new DataContractSerializer(typeof(Dictionary<string, object>), _knownTypes);
					serializer.WriteObject(stream, data);
					serializedData = stream.ToArray();
				}

				var container = new DataContainer
				{
					TypeNames = _knownTypes.Select(x => x.AssemblyQualifiedName).ToArray(),
					Data = serializedData
				};

				byte[] containerData;
				using (var stream = new MemoryStream())
				{
					var serializer = new DataContractSerializer(typeof(DataContainer));
					serializer.WriteObject(stream, container);
					containerData = stream.ToArray();
				}

				var provider = new DataProtectionProvider("LOCAL = user");
				var encryptedData = await provider.ProtectAsync(containerData.AsBuffer());

				var file = await folder.CreateFileAsync(_cryptFileName, CreationCollisionOption.ReplaceExisting);
				await FileIO.WriteBufferAsync(file, encryptedData);
			}
			catch (SerializationException ex)
			{
				Debug.WriteLine(ex);
			}
		}

		private async Task<IDictionary<string, object>> LoadAsync(IStorageFolder folder)
		{
			var file = await GetStorageFileAsync(_cryptFileName, folder);
			if (file == null)
				return null;

			var encryptedData = await FileIO.ReadBufferAsync(file);

			var provider = new DataProtectionProvider();
			var packedData = await provider.UnprotectAsync(encryptedData);

			try
			{
				DataContainer container;
				using (var stream = packedData.AsStream())
				{
					var serializer = new DataContractSerializer(typeof(DataContainer));
					container = (DataContainer)serializer.ReadObject(stream);
				}

				_knownTypes = (_knownTypes ?? Array.Empty<Type>())
					.Union(container.TypeNames.Select(x => Type.GetType(x)))
					.ToArray();

				using (var stream = new MemoryStream(container.Data))
				{
					var serializer = new DataContractSerializer(typeof(Dictionary<string, object>), _knownTypes);
					return (Dictionary<string, object>)serializer.ReadObject(stream);
				}
			}
			catch (SerializationException ex)
			{
				Debug.WriteLine(ex);
				return null;
			}
		}

		private async Task DeleteAsync(IStorageFolder folder)
		{
			var file = await GetStorageFileAsync(_cryptFileName, folder);
			if (file == null)
				return;

			await file.DeleteAsync();
		}

		private static async Task<IStorageFile> GetStorageFileAsync(string fileName, IStorageFolder folder)
		{
			var storageFolder = folder as StorageFolder;
			if (storageFolder != null)
			{
				return await storageFolder.TryGetItemAsync(fileName) as StorageFile;
			}

			try
			{
				return await folder.GetFileAsync(fileName);
			}
			catch (FileNotFoundException)
			{
				return null;
			}
		}

		private static IStorageFolder GetStorageFolder(bool isRoaming)
		{
			return !isRoaming
				? ApplicationData.Current.LocalFolder
				: ApplicationData.Current.RoamingFolder;
		}

		#endregion
	}
}