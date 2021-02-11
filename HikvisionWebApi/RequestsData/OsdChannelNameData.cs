using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Hikvision.RequestsData
{
	public class OsdChannelNameData
	{
		[JsonProperty( "channelNameOverlay" )] public ChannelNameOverlay channelNameOverlay { get; set; }
		public class ChannelNameOverlay
		{
			public bool enabled { get; set; }
			public short positionX { get; set; }
			public short positionY { get; set; }
		}

	}
}
