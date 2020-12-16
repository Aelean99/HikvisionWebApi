using System.Xml.Serialization;

namespace Hikvision.RequestsData
{
	public static class OsdData
	{
		public class channelNameOverlay
		{
			[XmlElement("enabled")] public bool Enabled = false;
			[XmlElement("positionX")] public short PositionX = 512;
			[XmlElement("positionY")] public short PositionY = 64;
		}

		public class dateTimeOverlay
		{
			[XmlElement("enabled")] public bool Enabled = false;
			[XmlElement("positionX")] public short PositionX = 0;
			[XmlElement("positionY")] public short PositionY = 544;
			[XmlElement("dateStyle")] public string DateStyle = "DD-MM-YYYY";
			[XmlElement("timeStyle")] public string TimeStyle = "24hour";
			[XmlElement("displayWeek")] public bool DisplayWeek = false;
		}
	}
}
