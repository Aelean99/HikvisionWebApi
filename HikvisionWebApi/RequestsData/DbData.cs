using Newtonsoft.Json;

using System.Collections.Generic;

namespace Hikvision.RequestsData
{
	public class DbData
	{
		[JsonProperty( "signature" )] public string Signature { get; set; }
		[JsonProperty( "data" )] public object? Data { get; set; }
		[JsonProperty( "message" )] public List<CameraData>? Message { get; set; }
	}


	public class CameraData
	{
		[JsonProperty( "id" )] public uint Id { get; set; }
		[JsonProperty( "active" )] public bool? Active { get; set; }
		[JsonProperty( "password" )] public string Password { get; set; }
		[JsonProperty( "screen_url" )] public ushort? screen_url { get; set; }
		[JsonProperty( "monitoring" )] public bool? Monitoring { get; set; }
		[JsonProperty( "mic" )] public bool? Mic { get; set; }
		[JsonProperty( "rtsp_ip" )] public string rtsp_ip { get; set; }
		[JsonProperty( "camera_type" )] public ushort? camera_type { get; set; }
		[JsonProperty( "camera_status" )] public ushort? camera_status { get; set; }
		[JsonProperty( "detection_mask" )] public string detection_mask { get; set; }
		[JsonProperty( "detection_status" )] public ushort? detection_status { get; set; }
		[JsonProperty( "mac" )] public string Mac { get; set; }
		[JsonProperty( "serial" )] public string Serial { get; set; }
	}
	public class CameraError
	{
		[JsonProperty( "id" )] public uint Id { get; set; }
		[JsonProperty( "camera_status" )] public ushort? camera_status { get; set; }
	}

	public class MaskError
	{
		[JsonProperty("id")] public uint Id { get; set; }
		[JsonProperty("detection_status")] public ushort? detection_status { get; set; }
	}

	public class CamId
	{
		[JsonProperty( "id" )] public uint Id { get; set; }
	}
}
