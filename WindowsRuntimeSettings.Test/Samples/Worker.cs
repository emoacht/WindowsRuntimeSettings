using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WindowsRuntimeSettings.Test.Samples
{
	[DataContract]
	public class Worker
	{
		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public SkillType Skill { get; set; }

		[DataMember]
		public Worker Parent { get; set; }
	}
}