using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using WindowsRuntimeSettings.Accessor;

namespace WindowsRuntimeSettings
{
	public abstract class SettingsBase : INotifyPropertyChanged
	{
		private readonly IPropertyAccessorProvider _provider;

		public SettingsBase() : this(new PropertyAccessorProvider())
		{ }

		internal SettingsBase(IPropertyAccessorProvider provider)
		{
			_provider = provider ?? throw new ArgumentNullException(nameof(provider));
		}

		#region Getter

		protected T GetValue<T>([CallerMemberName] string propertyName = null)
		{
			return _provider.GetAccessor<T>(this.GetType(), propertyName).GetValue<T>(propertyName);
		}

		protected T GetValue<T>(T defaultValue, [CallerMemberName] string propertyName = null)
		{
			return _provider.GetAccessor<T>(this.GetType(), propertyName).GetValue(defaultValue, propertyName);
		}

		#endregion

		#region Setter

		protected void SetValue<T>(T propertyValue, [CallerMemberName] string propertyName = null)
		{
			_provider.GetAccessor<T>(this.GetType(), propertyName).SetValue(propertyValue, propertyName);
		}

		#endregion

		#region INotifyPropertyChanged member

		public event PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged([CallerMemberName] string propertyName = null) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		#endregion
	}
}