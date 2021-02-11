using Newtonsoft.Json;

namespace Hikvision.RequestsData
{
	public class MassConfigData
	{
		[JsonProperty( "id" )] public uint Id { get; set; }
		[JsonProperty( "ntpData" )] public NtpData NtpData { get; set; }
		[JsonProperty( "timeData" )] public TimeData TimeData { get; set; }
		[JsonProperty( "osdChannelNameData" )] public OsdChannelNameData OsdChannelNameData { get; set; }
		[JsonProperty( "osdDatetimeData" )] public OsdDatetimeData OsdDateTimeData { get; set; }
		[JsonProperty( "emailData" )] public EmailData EmailData { get; set; }
		[JsonProperty( "streamingData" )] public StreamingData StreamingData { get; set; }
		[JsonProperty( "detectionData" )] public DetectionData DetectionData { get; set; }
		[JsonProperty( "eventTriggerData" )] public NotificationData EventTriggerData { get; set; }
		[JsonProperty( "networkData" )] public NetworkData NetworkData { get; set; }
	}

	public class CamId
	{
		[JsonProperty("id")]public uint Id { get; set; }
	}
}
