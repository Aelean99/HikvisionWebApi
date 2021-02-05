using System.Xml.Serialization;

namespace Hikvision.RequestsData
{
	public class TimeData
	{
		public class NTPServer
		{
			public string id { get; set; }
			public string addressingFormatType { get; set; }
			public string ipAddress { get; set; }
			public byte portNo { get; set; }
			public byte synchronizeInterval { get; set; }
		} 

		public class Time
		{
			public string timeMode { get; set; }
			public string timeZone { get; set; }
		}
	}
}
