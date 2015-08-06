using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsRuntimeSettings.Test.Samples
{
	public class Settings
	{
		#region Plain

		public string PlainLocalProperty { get; set; }

		[Roaming]
		public string PlainRoamingProperty { get; set; }

		[CryptVault]
		public string PlainCryptVaultProperty { get; set; }

		[CryptFile]
		public string PlainCryptFileLocalProperty { get; set; }

		[CryptFile]
		[Roaming]
		public string PlainCryptFileRoamingProperty { get; set; }

		#endregion

		#region Enum

		public SkillType EnumLocalProperty { get; set; }

		[Roaming]
		public SkillType EnumRoamingProperty { get; set; }

		[CryptVault]
		public SkillType EnumCryptVaultProperty { get; set; }

		[CryptFile]
		public SkillType EnumCryptFileLocalProperty { get; set; }

		[CryptFile]
		[Roaming]
		public SkillType EnumCryptFileRoamingProperty { get; set; }

		#endregion

		#region DataContract

		public Worker DataContractLocalProperty { get; set; }

		[Roaming]
		public Worker DataContractRoamingProperty { get; set; }

		[CryptVault]
		public Worker DataContractCryptVaultLocalProperty { get; set; }

		[CryptFile]
		public Worker DataContractCryptFileLocalProperty { get; set; }

		[CryptFile]
		[Roaming]
		public Worker DataContractCryptFileRoamingProperty { get; set; }

		#endregion
	}
}