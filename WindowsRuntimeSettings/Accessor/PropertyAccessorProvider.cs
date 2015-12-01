using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace WindowsRuntimeSettings.Accessor
{
	internal class PropertyAccessorProvider : IPropertyAccessorProvider
	{
		#region Constant

		/// <summary>
		/// Windows Runtime base data types supported for the application settings
		/// </summary>
		/// <remarks>
		/// https://msdn.microsoft.com/en-us/library/br205768.aspx
		/// Excluded Uri and void types. Included byte[] type.
		/// </remarks>
		private static readonly Type[] _baseTypes = new[]
		{
			typeof(bool),
			typeof(byte),
			typeof(char),
			typeof(DateTimeOffset),
			typeof(double),
			typeof(Guid),
			typeof(short),
			typeof(int),
			typeof(long),
			typeof(object),
			typeof(Point),
			typeof(Rect),
			typeof(float),
			typeof(Size),
			typeof(string),
			typeof(TimeSpan),
			typeof(ushort),
			typeof(uint),
			typeof(ulong),
			typeof(byte[])
		};

		#endregion

		private const string _cryptVaultName = "Settings";
		private const string _cryptFileName = "Settings";

		private readonly Dictionary<string, IPropertyAccessor> _accessorMap = new Dictionary<string, IPropertyAccessor>();
		private readonly List<IPropertyAccessor> _accessorRepository = new List<IPropertyAccessor>();

		private readonly Lazy<IRootAccessor> _settingsRootAccessor;
		private readonly Lazy<IRootAccessor> _cryptVaultRootAccessor;
		private readonly Lazy<IRootAccessor> _cryptFileRootAccessor;

		public PropertyAccessorProvider()
			: this(_cryptVaultName, _cryptFileName)
		{ }

		public PropertyAccessorProvider(string cryptVaultName, string cryptFileName)
		{
			if (string.IsNullOrWhiteSpace(cryptVaultName))
				throw new ArgumentNullException(nameof(cryptVaultName));
			if (string.IsNullOrWhiteSpace(cryptFileName))
				throw new ArgumentNullException(nameof(cryptFileName));

			_settingsRootAccessor = new Lazy<IRootAccessor>(() => new SettingsRootAccessor());
			_cryptVaultRootAccessor = new Lazy<IRootAccessor>(() => new CryptVaultRootAccessor(cryptVaultName));
			_cryptFileRootAccessor = new Lazy<IRootAccessor>(() => new CryptFileRootAccessor(cryptFileName));
		}

		public IPropertyAccessor GetAccessor<T>(Type classType, string propertyName)
		{
			if (_accessorMap.ContainsKey(propertyName))
				return _accessorMap[propertyName];

			if (classType == null)
				throw new ArgumentNullException(nameof(classType));

			var propertyInfo = classType.GetRuntimeProperty(propertyName);
			if (propertyInfo == null)
				throw new ArgumentException(nameof(propertyName));

			var rootAccessor = GetRootAccessor(propertyInfo);
			var isRoaming = propertyInfo.IsDefined(typeof(RoamingAttribute));

			var propertyAccessor = GetPropertyAccessor<T>(rootAccessor, isRoaming);

			_accessorMap.Add(propertyName, propertyAccessor);
			return propertyAccessor;
		}

		private IRootAccessor GetRootAccessor(PropertyInfo propertyInfo)
		{
			if (propertyInfo.IsDefined(typeof(CryptVaultAttribute)))
			{
				return _cryptVaultRootAccessor.Value;
			}
			if (propertyInfo.IsDefined(typeof(CryptFileAttribute)))
			{
				return _cryptFileRootAccessor.Value;
			}

			return _settingsRootAccessor.Value;
		}

		private IPropertyAccessor GetPropertyAccessor<T>(IRootAccessor rootAccessor, bool isRoaming)
		{
			if (_baseTypes.Contains(typeof(T)))
			{
				return FindPropertyAccessor<PropertyAccessor>(rootAccessor, isRoaming);
			}
			if (typeof(T).GetTypeInfo().IsEnum)
			{
				return FindPropertyAccessor<EnumPropertyAccessor>(rootAccessor, isRoaming);
			}

			return FindPropertyAccessor<DataContractPropertyAccessor>(rootAccessor, isRoaming);
		}

		private IPropertyAccessor FindPropertyAccessor<T>(IRootAccessor rootAccessor, bool isRoaming) where T : IPropertyAccessor
		{
			var accessor = _accessorRepository.FirstOrDefault(x =>
					(x.GetType() == typeof(T)) &&
					(x.RootAccessor == rootAccessor) &&
					(x.IsRoaming == isRoaming));

			if (accessor == null)
			{
				accessor = (IPropertyAccessor)Activator.CreateInstance(typeof(T), rootAccessor, isRoaming);
				_accessorRepository.Add(accessor);
			}

			return accessor;
		}
	}
}