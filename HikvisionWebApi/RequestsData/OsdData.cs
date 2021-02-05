using System.Xml.Serialization;

namespace Hikvision.RequestsData
{
	public class OsdData
	{
		public class channelNameOverlay
		{
			public bool enabled { get; set; }
			public short positionX { get; set; }
			public short positionY { get; set; }
		}

		public class dateTimeOverlay
		{
			public bool enabled { get; set; }
			public short positionX { get; set; }
			public short positionY { get; set; }
			public string dateStyle { get; set; }
			public string timeStyle { get; set; }
			public bool displayWeek { get; set; }
		}
	}
}
