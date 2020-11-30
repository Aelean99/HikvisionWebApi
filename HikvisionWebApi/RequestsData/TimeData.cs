using System.Xml.Serialization;

namespace Hikvision.RequestsData
{
	public static class TimeData
	{
		public class NTPServer
		{
			[XmlElement("id")] public byte Id = 1;
			[XmlElement("addressingFormatType")] public string AddressingFormatType { get; set; }
			[XmlElement("ipAddress")] public string IpAddress { get; set; }
			[XmlElement("portNo")] public byte PortNo = 123;
			[XmlElement("synchronizeInterval")] public byte SynchronizeInterval = 30;
		} 

		public class Time
		{
			[XmlElement("timeMode")] public string TimeMode = "NTP";
			[XmlElement("timeZone")] public string TimeZone { get; set; }
		}
	}
}
