using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsRuntimeSettings.Accessor
{
	internal interface IPropertyAccessor
	{
		IRootAccessor RootAccessor { get; }
		bool IsRoaming { get; }

		T GetValue<T>(string propertyName);
		T GetValue<T>(T defaultValue, string propertyName);
		bool TryGetValue<T>(out T propertyValue, string propertyName);

		void SetValue<T>(T propertyValue, string propertyName);
	}
}