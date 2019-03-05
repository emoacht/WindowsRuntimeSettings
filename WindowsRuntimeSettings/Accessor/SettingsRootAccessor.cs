using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace WindowsRuntimeSettings.Accessor
{
	/// <summary>
	/// Save/Load settings in LocalSettings/RoamingSettings.
	/// </summary>
	internal class SettingsRootAccessor : IRootAccessor
	{
		#region Getter

		public T GetValue<T>(string propertyName, bool isRoaming)
		{
			TryGetValue(out T propertyValue, propertyName, isRoaming);
			return propertyValue;
		}

		public T GetValue<T>(T defaultValue, string propertyName, bool isRoaming)
		{
			if (TryGetValue(out T propertyValue, propertyName, isRoaming))
				return propertyValue;
			else
				return defaultValue;
		}

		public bool TryGetValue<T>(out T propertyValue, string propertyName, bool isRoaming)
		{
			var values = GetSettingsValues(isRoaming);
			if (values.ContainsKey(propertyName))
			{
				propertyValue = (T)values[propertyName];
				return true;
			}
			else
			{
				propertyValue = default(T);
				return false;
			}
		}

		#endregion

		#region Setter

		public void SetValue<T>(T propertyValue, string propertyName, bool isRoaming)
		{
			var values = GetSettingsValues(isRoaming);
			if (values.ContainsKey(propertyName))
			{
				if (!EqualityComparer<T>.Default.Equals(propertyValue, default(T)))
				{
					values[propertyName] = propertyValue;
				}
				else
				{
					values.Remove(propertyName);
				}
			}
			else
			{
				if (!EqualityComparer<T>.Default.Equals(propertyValue, default(T)))
				{
					values.Add(propertyName, propertyValue);
				}
			}
		}

		#endregion

		#region Access

		private static IPropertySet GetSettingsValues(bool isRoaming)
		{
			return !isRoaming
				? ApplicationData.Current.LocalSettings.Values
				: ApplicationData.Current.RoamingSettings.Values;
		}

		#endregion
	}
}