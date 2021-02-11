using Newtonsoft.Json;

namespace Hikvision.RequestsData
{
	public class NtpData
	{
		[JsonProperty( "NTPServer" )] public NTPServer ntpServer { get; set; }
		public class NTPServer
		{
			public string id { get; set; }
			public string addressingFormatType { get; set; }
			public string ipAddress { get; set; }
			public byte portNo { get; set; }
			public byte synchronizeInterval { get; set; }
		}
	}
}
