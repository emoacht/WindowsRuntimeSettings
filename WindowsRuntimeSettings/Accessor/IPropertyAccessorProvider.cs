using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsRuntimeSettings.Accessor
{
	internal interface IPropertyAccessorProvider
	{
		IPropertyAccessor GetAccessor<T>(Type classType, string propertyName);
	}
}