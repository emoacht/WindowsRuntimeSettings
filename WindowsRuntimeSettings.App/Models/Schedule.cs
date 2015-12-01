using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WindowsRuntimeSettings.App.Models
{
	[DataContract]
	public class Schedule
	{
		[DataMember]
		public TransportationType Transportation { get; set; }

		[DataMember]
		public string DestinationName { get; set; }

		[DataMember]
		public DateTime DepatureTime { get; set; }

		[DataMember]
		public TimeSpan TravelTime { get; set; }

		[DataMember]
		public double Distance { get; set; }
	}
}