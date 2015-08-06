using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsRuntimeSettings.Accessor
{
	internal interface IRootAccessor
	{
		T GetValue<T>(string propertyName, bool isRoaming);
		T GetValue<T>(T defaultValue, string propertyName, bool isRoaming);
		bool TryGetValue<T>(out T propertyValue, string propertyName, bool isRoaming);

		void SetValue<T>(T propertyValue, string propertyName, bool isRoaming);
	}
}