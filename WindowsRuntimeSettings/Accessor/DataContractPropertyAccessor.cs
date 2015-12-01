using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WindowsRuntimeSettings.Accessor
{
	/// <summary>
	/// Save/Load settings using data contract.
	/// </summary>
	internal class DataContractPropertyAccessor : IPropertyAccessor
	{
		public DataContractPropertyAccessor()
			: this(new SettingsRootAccessor())
		{ }

		public DataContractPropertyAccessor(IRootAccessor accessor, bool isRoaming = false)
		{
			if (accessor == null)
				throw new ArgumentNullException(nameof(accessor));

			_accessor = accessor;
			_isRoaming = isRoaming;
		}

		public IRootAccessor RootAccessor => _accessor;
		private readonly IRootAccessor _accessor;

		public bool IsRoaming => _isRoaming;
		private readonly bool _isRoaming;

		#region Getter

		public T GetValue<T>(string propertyName)
		{
			T propertyValue;
			TryGetValue(out propertyValue, propertyName);
			return propertyValue;
		}

		public T GetValue<T>(T defaultValue, string propertyName)
		{
			T propertyValue;
			if (TryGetValue(out propertyValue, propertyName))
				return propertyValue;
			else
				return defaultValue;
		}

		public bool TryGetValue<T>(out T propertyValue, string propertyName)
		{
			byte[] serializedData;
			if (_accessor.TryGetValue(out serializedData, propertyName, _isRoaming))
			{
				using (var ms = new MemoryStream())
				{
					ms.Write(serializedData, 0, serializedData.Length);
					ms.Seek(0, SeekOrigin.Begin);

					// If DataContractJsonSerializer is used, DateTime and DateTimeOffset values will not
					// be able to fully restored because small scale of values will be rounded and lost.
					var serializer = new DataContractSerializer(typeof(T));
					propertyValue = (T)serializer.ReadObject(ms);
					return true;
				}
			}
			else
			{
				propertyValue = default(T);
				return false;
			}
		}

		#endregion

		#region Setter

		public void SetValue<T>(T propertyValue, string propertyName)
		{
			using (var ms = new MemoryStream())
			{
				// If DataContractJsonSerializer is used, DateTime and DateTimeOffset values will not
				// be able to fully restored because small scale of values will be rounded and lost.
				var serializer = new DataContractSerializer(typeof(T));
				serializer.WriteObject(ms, propertyValue);

				_accessor.SetValue(ms.ToArray(), propertyName, _isRoaming);
			}
		}

		#endregion
	}
}