using Newtonsoft.Json;

namespace Hikvision.RequestsData
{
	public class StreamingData
	{
		[JsonProperty( "StreamingChannel")] public StreamingChannel streamingChannel { get; set; }
		public class StreamingChannel
		{
			public Video Video { get; set; }
			public Audio Audio { get; set; }
		}

		public class Video
		{
			public string videoCodecType { get; set; }
			public int videoResolutionWidth { get; set; }
			public int videoResolutionHeight { get; set; }
			public string videoQualityControlType { get; set; }
			public byte fixedQuality { get; set; }
			public int maxFrameRate { get; set; }
			public int vbrUpperCap { get; set; }
			public byte GovLength { get; set; }
		}

		public class Audio
		{
			public bool enabled { get; set; }
			public string audioCompressionType { get; set; }
		}
	}
}
