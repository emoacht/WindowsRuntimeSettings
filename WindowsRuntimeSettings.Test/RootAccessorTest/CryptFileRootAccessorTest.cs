using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.Foundation;
using Windows.UI;

using WindowsRuntimeSettings.Accessor;
using WindowsRuntimeSettings.Test.Samples;

namespace WindowsRuntimeSettings.Test.RootAccessorTest
{
	[TestClass]
	public class CryptFileRootAccessorTest
	{
		private static CryptFileRootAccessor _accessor;

		[ClassInitialize]
		public static void Initialize(TestContext context)
		{
			_accessor = new CryptFileRootAccessor("CryptFileTestSettings");
		}

		[TestMethod]
		public void TestPrimitive()
		{
			TestPrimitiveBase(true, "BoolValue", false);
			TestPrimitiveBase((byte)128, "ByteValue", false);
			TestPrimitiveBase('c', "CharValue", false);
			TestPrimitiveBase(DateTimeOffset.UtcNow, "DateTimeOffsetValue", false);
			TestPrimitiveBase(256.78D, "DoubleValue", false);
			TestPrimitiveBase(Guid.NewGuid(), "GuidValue", false);
			TestPrimitiveBase((short)64, "ShortValue", false);
			TestPrimitiveBase(167, "IntValue", false);
			TestPrimitiveBase(4294967294L, "LongValue", false);
			TestPrimitiveBase((object)56, "ObjectValue", false);
			TestPrimitiveBase(new Point(100, 100), "PointValue", false);
			TestPrimitiveBase(new Rect(0, 0, 128, 66), "RectValue", false);
			TestPrimitiveBase(1.568F, "FloatValue", false);
			TestPrimitiveBase(new Size(128, 88), "SizeValue", false);
			TestPrimitiveBase("Haldir", "StringValue", false);
			TestPrimitiveBase(TimeSpan.FromHours(8.6), "TimeSpanValue", false);
			TestPrimitiveBase((ushort)64, "UshortValue", false);
			TestPrimitiveBase(334U, "UIntValue", false);
			TestPrimitiveBase(8589934588UL, "ULongValue", false);
		}

		private void TestPrimitiveBase<T>(T propertyValue, string propertyName, bool isRoaming)
		{
			_accessor.SetValue(propertyValue, propertyName, isRoaming);
			Assert.AreEqual(propertyValue, _accessor.GetValue<T>(propertyName, isRoaming));
		}

		[TestMethod]
		public void TestDateTime()
		{
			var name = "DateTimeValue";
			var value = DateTime.Now;

			_accessor.SetValue(value, name, false);
			Assert.AreEqual(value, _accessor.GetValue<DateTime>(name, false));
		}

		[TestMethod]
		public void TestColor()
		{
			var name = "ColorValue";
			var value = Colors.Chocolate;

			_accessor.SetValue(value, name, false);
			Assert.AreEqual(value, _accessor.GetValue<Color>(name, false));
		}

		[TestMethod]
		public void TestByteArray()
		{
			var name = "BytesValue";
			var value = Encoding.UTF8.GetBytes("BytesValue");

			_accessor.SetValue(value, name, false);
			Assert.IsTrue(value.SequenceEqual(_accessor.GetValue<byte[]>(name, false)));
		}

		[TestMethod]
		public void TestCustomClass()
		{
			var name = "ClassValue";
			var value = new Worker
			{
				Id = 13279,
				Name = "Little Can",
				Skill = SkillType.Tinker,
				Parent = new Worker { Name = "Big Can" }
			};

			_accessor.SetValue(value, name, false);
			Assert.AreEqual(value.Name, _accessor.GetValue<Worker>(name, false).Name);
			Assert.AreEqual(value.Skill, _accessor.GetValue<Worker>(name, false).Skill);
		}

		[TestMethod]
		public void TestRemove()
		{
			var name = "StrigValue";

			_accessor.SetValue<string>(null, name, false);
			string value;
			Assert.IsFalse(_accessor.TryGetValue(out value, name, false));
			Assert.AreEqual(null, value);
		}

		[TestMethod]
		public void TestRoaming()
		{
			var commonName = "CommonName";
			var localValue = "LocalValue";
			var roamingValue = "RoamingValue";

			_accessor.SetValue(localValue, commonName, false);
			_accessor.SetValue(roamingValue, commonName, true);

			Assert.AreEqual(localValue, _accessor.GetValue<string>(commonName, false));
			Assert.AreNotEqual(localValue, _accessor.GetValue<string>(commonName, true));

			Assert.AreEqual(roamingValue, _accessor.GetValue<string>(commonName, true));
			Assert.AreNotEqual(roamingValue, _accessor.GetValue<string>(commonName, false));
		}

		[TestMethod]
		public void TestDefault()
		{
			TestDefaultBase("World", "StringValue");
			TestDefaultBase(180, "IntValue");
			TestDefaultBase(DateTimeOffset.Now, "DateTimeOffsetValue");
		}

		private void TestDefaultBase<T>(T defaultValue, string propertyName)
		{
			_accessor.SetValue(default(T), propertyName, false);
			T value;
			Assert.IsFalse(_accessor.TryGetValue(out value, propertyName, false));

			Assert.AreEqual(default(T), _accessor.GetValue<T>(propertyName, false));
			Assert.AreEqual(defaultValue, _accessor.GetValue(defaultValue, propertyName, false));
		}
	}
}