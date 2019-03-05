using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.Foundation;
using Windows.UI;

using WindowsRuntimeSettings.Accessor;

namespace WindowsRuntimeSettings.Test.RootAccessorTest
{
	[TestClass]
	public class SettingsRootAccessorTest
	{
		private static SettingsRootAccessor _accessor;

		[ClassInitialize]
		public static void Initialize(TestContext context)
		{
			_accessor = new SettingsRootAccessor();
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
		public void TestUriException()
		{
			var name = "UriValue";
			var value = new Uri("http://www.visualstudio.com/");

			Assert.ThrowsException<COMException>(() => _accessor.SetValue(value, name, false));
		}

		[TestMethod]
		public void TestDateTimeException()
		{
			var name = "DateTimeValue";
			var value = DateTime.Now;

			Assert.ThrowsException<COMException>(() => _accessor.SetValue(value, name, false));
		}

		[TestMethod]
		public void TestColorException()
		{
			var name = "ColorValue";
			var value = Colors.Chocolate;

			Assert.ThrowsException<COMException>(() => _accessor.SetValue(value, name, false));
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
		public void TestRemove()
		{
			var name = "StringValue";

			_accessor.SetValue<string>(null, name, false);
			Assert.IsFalse(_accessor.TryGetValue(out string value, name, false));
			Assert.AreEqual(null, value);
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
			Assert.IsFalse(_accessor.TryGetValue(out T value, propertyName, false));

			Assert.AreEqual(default(T), _accessor.GetValue<T>(propertyName, false));
			Assert.AreEqual(defaultValue, _accessor.GetValue(defaultValue, propertyName, false));
		}
	}
}