using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsRuntimeSettings.Accessor
{
	/// <summary>
	/// Save/Load settings of enum type.
	/// </summary>
	internal class EnumPropertyAccessor : IPropertyAccessor
	{
		public EnumPropertyAccessor()
			: this(new SettingsRootAccessor())
		{ }

		public EnumPropertyAccessor(IRootAccessor accessor, bool isRoaming = false)
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
			return _accessor.GetValue<T>(propertyName, _isRoaming);
		}

		public T GetValue<T>(T defaultValue, string propertyName)
		{
			return _accessor.GetValue(defaultValue, propertyName, _isRoaming);
		}

		public bool TryGetValue<T>(out T propertyValue, string propertyName)
		{
			return _accessor.TryGetValue(out propertyValue, propertyName, _isRoaming);
		}

		#endregion

		#region Setter

		public void SetValue<T>(T propertyValue, string propertyName)
		{
			var underlyingValue = Convert.ChangeType(propertyValue, Enum.GetUnderlyingType(typeof(T)));

			_accessor.SetValue(underlyingValue, propertyName, _isRoaming);
		}

		#endregion
	}
}