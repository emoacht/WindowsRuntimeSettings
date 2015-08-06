using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

using WindowsRuntimeSettings.Accessor;
using WindowsRuntimeSettings.Test.Samples;

namespace WindowsRuntimeSettings.Test.PropertyAccessorProviderTest
{
	[TestClass]
	public class PropertyAccessorProviderTest
	{
		private static PropertyAccessorProvider _provider;

		[ClassInitialize]
		public static void Initialize(TestContext context)
		{
			_provider = new PropertyAccessorProvider("ProviderTestSettings", "ProviderTestSettings");
		}

		[TestMethod]
		public void TestPlain()
		{
			TestBase<string>(typeof(Settings), nameof(Settings.PlainLocalProperty), typeof(PropertyAccessor), typeof(SettingsRootAccessor), false);
			TestBase<string>(typeof(Settings), nameof(Settings.PlainRoamingProperty), typeof(PropertyAccessor), typeof(SettingsRootAccessor), true);
			TestBase<string>(typeof(Settings), nameof(Settings.PlainCryptVaultProperty), typeof(PropertyAccessor), typeof(CryptVaultRootAccessor), false);
			TestBase<string>(typeof(Settings), nameof(Settings.PlainCryptFileLocalProperty), typeof(PropertyAccessor), typeof(CryptFileRootAccessor), false);
			TestBase<string>(typeof(Settings), nameof(Settings.PlainCryptFileRoamingProperty), typeof(PropertyAccessor), typeof(CryptFileRootAccessor), true);
		}

		[TestMethod]
		public void TestEnum()
		{
			TestBase<SkillType>(typeof(Settings), nameof(Settings.EnumLocalProperty), typeof(EnumPropertyAccessor), typeof(SettingsRootAccessor), false);
			TestBase<SkillType>(typeof(Settings), nameof(Settings.EnumRoamingProperty), typeof(EnumPropertyAccessor), typeof(SettingsRootAccessor), true);
			TestBase<SkillType>(typeof(Settings), nameof(Settings.EnumCryptVaultProperty), typeof(EnumPropertyAccessor), typeof(CryptVaultRootAccessor), false);
			TestBase<SkillType>(typeof(Settings), nameof(Settings.EnumCryptFileLocalProperty), typeof(EnumPropertyAccessor), typeof(CryptFileRootAccessor), false);
			TestBase<SkillType>(typeof(Settings), nameof(Settings.EnumCryptFileRoamingProperty), typeof(EnumPropertyAccessor), typeof(CryptFileRootAccessor), true);
		}

		[TestMethod]
		public void TestDataContract()
		{
			TestBase<Worker>(typeof(Settings), nameof(Settings.DataContractLocalProperty), typeof(DataContractPropertyAccessor), typeof(SettingsRootAccessor), false);
			TestBase<Worker>(typeof(Settings), nameof(Settings.DataContractRoamingProperty), typeof(DataContractPropertyAccessor), typeof(SettingsRootAccessor), true);
			TestBase<Worker>(typeof(Settings), nameof(Settings.DataContractCryptVaultLocalProperty), typeof(DataContractPropertyAccessor), typeof(CryptVaultRootAccessor), false);
			TestBase<Worker>(typeof(Settings), nameof(Settings.DataContractCryptFileLocalProperty), typeof(DataContractPropertyAccessor), typeof(CryptFileRootAccessor), false);
			TestBase<Worker>(typeof(Settings), nameof(Settings.DataContractCryptFileRoamingProperty), typeof(DataContractPropertyAccessor), typeof(CryptFileRootAccessor), true);
		}

		private void TestBase<T>(Type classType, string propertyName,
			Type propertyAccessorType, Type rootAccessorType, bool isRoaming)
		{
			var accessor = _provider.GetAccessor<T>(classType, propertyName);

			Assert.AreEqual(propertyAccessorType, accessor.GetType());
			Assert.AreEqual(rootAccessorType, accessor.RootAccessor.GetType());
			Assert.AreEqual(isRoaming, accessor.IsRoaming);
		}
	}
}