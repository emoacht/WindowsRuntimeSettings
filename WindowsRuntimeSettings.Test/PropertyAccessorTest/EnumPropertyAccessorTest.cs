using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WindowsRuntimeSettings.Accessor;
using WindowsRuntimeSettings.Test.Samples;

namespace WindowsRuntimeSettings.Test.PropertyAccessorTest
{
	[TestClass]
	public class EnumPropertyAccessorTest
	{
		private static EnumPropertyAccessor _accessor;

		[ClassInitialize]
		public static void Initialize(TestContext context)
		{
			_accessor = new EnumPropertyAccessor();
		}

		[TestMethod]
		public void TestCustomEnum()
		{
			var name = "EnumValue";
			var value = SkillType.Carpenter;

			_accessor.SetValue(value, name);
			Assert.AreEqual(value, _accessor.GetValue<SkillType>(name));
		}
	}
}