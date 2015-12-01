using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.UI;

using WindowsRuntimeSettings.Accessor;
using WindowsRuntimeSettings.Test.Samples;

namespace WindowsRuntimeSettings.Test.PropertyAccessorTest
{
	[TestClass]
	public class DataContractPropertyAccessorTest
	{
		private static DataContractPropertyAccessor _accessor;

		[ClassInitialize]
		public static void Initialize(TestContext context)
		{
			_accessor = new DataContractPropertyAccessor();
		}

		[TestMethod]
		public void TestUri()
		{
			var name = "UriValue";
			var value = new Uri("http://www.visualstudio.com/");

			_accessor.SetValue(value, name);
			Assert.AreEqual(value, _accessor.GetValue<Uri>(name));
		}

		[TestMethod]
		public void TestDateTimeOffset()
		{
			var name = "DateTimeOffsetValue";
			var value = DateTimeOffset.Now;

			_accessor.SetValue(value, name);
			Assert.AreEqual(value.Ticks, _accessor.GetValue<DateTimeOffset>(name).Ticks);
		}

		[TestMethod]
		public void TestDateTime()
		{
			var name = "DateTimeValue";
			var value = DateTime.Now;

			_accessor.SetValue(value, name);
			Assert.AreEqual(value, _accessor.GetValue<DateTime>(name));
		}

		[TestMethod]
		public void TestColor()
		{
			var name = "ColorValue";
			var value = Colors.Chocolate;

			_accessor.SetValue(value, name);
			Assert.AreEqual(value, _accessor.GetValue<Color>(name));
		}

		[TestMethod]
		public void TestCustomClass()
		{
			var name = "ClassValue";
			var value = new Worker
			{
				Name = "Gandalf",
				Id = 100,
				Skill = SkillType.Weaver
			};

			_accessor.SetValue(value, name);
			Assert.AreEqual(value.Name, _accessor.GetValue<Worker>(name).Name);
			Assert.AreEqual(value.Id, _accessor.GetValue<Worker>(name).Id);
			Assert.AreEqual(value.Skill, _accessor.GetValue<Worker>(name).Skill);
		}
	}
}