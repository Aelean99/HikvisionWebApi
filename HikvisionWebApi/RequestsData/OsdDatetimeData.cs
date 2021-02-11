using Newtonsoft.Json;

namespace Hikvision.RequestsData
{
	public class OsdDatetimeData
	{
		[JsonProperty( "OsdDatetime" )] public OsdDatetime osdDateTime { get; set; }
		public class OsdDatetime
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
