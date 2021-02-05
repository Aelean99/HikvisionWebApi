namespace Hikvision.RequestsData
{
	public class MassConfigData
	{
		public uint id { get; set; }
		public TimeData.NTPServer ntpData { get; set; }
		public TimeData.Time timeData { get; set; }
		public OsdData.channelNameOverlay osdChannelNameData { get; set; }
		public OsdData.dateTimeOverlay osdDateTimeData { get; set; }
		public EmailData.Mailing emailData { get; set; }
		public StreamingData.StreamingChannel streamingData { get; set; }
		public DetectionData.MotionDetection detectionData { get; set; }
		public DetectionData.EventTriggerNotificationList eventTriggerData { get; set; }
		public NetworkData.RootData networkData { get; set; }
	}
}
