using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hikvision.RequestsData
{
	public static class StreamingData
	{
		public class StreamingChannel
		{
			[XmlElement("Video")] public Video Video { get; set; }
			[XmlElement("Audio")] public Audio Audio { get; set; }
		}

		public class Video
		{
			[XmlElement("videoCodecType")] public string VideoCodecType { get; set; }
			[XmlElement("videoResolutionWidth")] public int VideoResolutionWidth { get; set; }
			[XmlElement("videoResolutionHeight")] public int VideoResolutionHeight { get; set; }
			[XmlElement("videoQualityControlType")] public string VideoQualityControlType { get; set; }
			[XmlElement("fixedQuality")] public byte FixedQuality { get; set; }
			[XmlElement( "maxFrameRate" )] public int MaxFrameRate { get; set; }
			[XmlElement("vbrUpperCap")] public int VbrUpperCap { get; set; }
			[XmlElement("GovLength")] public byte GovLength { get; set; }
		}

		public class Audio
		{
			[XmlElement("enabled")] public bool Enabled { get; set; }
			[XmlElement("audioCompressionType")] public string AudioCompressionType { get; set; }
		}
	}
}
