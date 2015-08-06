using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsRuntimeSettings.Accessor
{
	internal interface IStringConverter
	{
		string ConvertToString<T>(T value);
		T ConvertFromString<T>(string value);
	}
}