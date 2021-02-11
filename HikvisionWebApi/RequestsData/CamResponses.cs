using Newtonsoft.Json;

namespace Hikvision.RequestsData
{
	public class CamResponses
	{
		[JsonProperty( "DeviceInfo" )] public DeviceInfo deviceInfo { get; set; }
		public class DeviceInfo
		{
			public string serialNumber { get; set; }
			public string macAddress { get; set; }
		}

		[JsonProperty( "userCheck" )] public UserCheck userCheck { get; set; }
		public class UserCheck
		{
			[JsonProperty( "statusValue" )] public int StatusValue { get; set; }
			[JsonProperty( "statusString" )] public string AuthStatus { get; set; }
			[JsonProperty( "lockStatus" )] public string LockStatus { get; set; }
			[JsonProperty( "retryLoginTime" )] public int AvailableLoginCount { get; set; }
		}

		[JsonProperty("ResponseStatus")] public ResponseStatus responseStatus { get; set; }
		public class ResponseStatus
		{
			[JsonProperty( "requestURL" )] public string RequestUrl { get; set; }
			[JsonProperty( "statusCode" )] public int StatusCode { get; set; }
			[JsonProperty( "statusString" )] public string StatusString { get; set; }
			[JsonProperty( "subStatusCode" )] public string SubStatusCode { get; set; }

		}
	}
}
