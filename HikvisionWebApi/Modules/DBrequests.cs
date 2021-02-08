using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hikvision.Modules
{
	public class DBrequests
	{
		public static JObject dbconf = JObject.Parse( File.ReadAllText( "dbvalues.json" ) );
		public static HttpClient client = new( new SocketsHttpHandler(), false ) 
		{
			BaseAddress = new Uri((string) dbconf["url"])
		};

		public class DbData
		{
			[JsonProperty( "signature" )]public string Signature { get; set; }
			[JsonProperty("data")]public object? Data { get; set; }
			[JsonProperty( "message" )]public List<CameraData> Message { get; set; }
		}

		public class CameraData
		{	
			[JsonProperty( "id" )]public uint? Id { get; set; }
			[JsonProperty( "active" )] public bool? Active { get; set; }
			[JsonProperty( "password" )] public string Password { get; set; }
			[JsonProperty( "screen_url" )] public ushort? ScreenUrl { get; set; }
			[JsonProperty( "monitoring" )] public bool? Monitoring { get; set; }
			[JsonProperty( "mic" )] public bool? Mic { get; set; }
			[JsonProperty( "rtsp_ip" )] public string RtspIp { get; set; }
			[JsonProperty( "camera_type" )] public ushort? CameraType { get; set; }
			[JsonProperty( "camera_status" )] public ushort? CameraStatus { get; set; }
			[JsonProperty( "settings" )] public ushort? Settings { get; set; }
			[JsonProperty( "detection_mask" )] public string DetectionMask { get; set; }
			[JsonProperty( "detection_status" )] public ushort? DetectionStatus { get; set; }
			[JsonProperty( "mac" )] public string Mac { get; set; }
			[JsonProperty( "serial")] public string Serial { get; set; }
		}

		public class CamId
		{
			[JsonProperty( "id" )] public uint? Id { get; set; }
		}

		public static async Task<string> CameraEdit(uint id, bool audioEnabled)
		{
			var jObject = Requests.ToJObject(await Requests.DeviceInfo());
			var serialNo = (string)jObject["DeviceInfo"]?["serialNumber"];
			var macAddress = (string)jObject["DeviceInfo"]?["macAddress"];
			var data = new DbData
			{
				Signature = (string)dbconf?["signature"],
				Data = new CameraData
				{
					DetectionMask = 
						"1111111111111111111111" +
						"1111111111111111111111" +
						"1111111111111111111111" +
						"1111111111111111111111" +
						"1111111111111111111111" +
						"1111111111111111111111" +
						"1111111111111111111111" +
						"1111111111111111111111" +
						"1111111111111111111111" +
						"1111111111111111111111" +
						"1111111111111111111111" +
						"1111111111111111111111" +
						"1111111111111111111111" +
						"1111111111111111111111" +
						"1111111111111111111111" +
						"1111111111111111111111" +
						"1111111111111111111111" +
						"1111111111111111111111",
					Id = id,
					Active = true,
					Settings = audioEnabled ? 61 : 48,
					CameraStatus = 105,
					CameraType = 1,
					ScreenUrl = 2,
					DetectionStatus = 205,
					Monitoring = true,
					Password = (string)dbconf["password"],
					Mac = macAddress,
					Serial = serialNo,
				}
			};
			JsonContent content = JsonContent.Create(data);
			return await client.PostAsync((string)dbconf["cameraEdit"], content).Result.Content.ReadAsStringAsync();
		}

		public static async Task<List<CameraData>> CameraGet(uint id)
		{
			var data = new DbData()
			{
				Signature = (string) dbconf["signature"],
				Data = new CamId { Id = id }
			};

			JsonContent content = JsonContent.Create(data);
			var response = await client.PostAsync((string) dbconf["cameraGet"], content).Result.Content.ReadAsStringAsync();
			var deserializedResponce = JsonConvert.DeserializeObject<DbData>( response );
			return deserializedResponce.Message;
		}
	}
}
