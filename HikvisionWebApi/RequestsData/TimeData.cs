using System.Xml.Serialization;

namespace Hikvision.RequestsData
{
	public static class TimeData
	{
		public class NTPServer
		{
			public byte id { get; set; }
			public string addressingFormatType { get; set; }
			public string ipAddress { get; set; }
			public byte portNo { get; set; }
			public byte synchronizeInterval { get; set; }
		} 

		public class Time
		{
			[XmlElement("timeMode")] public string TimeMode { get; set; }
			[XmlElement("timeZone")] public string TimeZone { get; set; }
		}
	}
}
