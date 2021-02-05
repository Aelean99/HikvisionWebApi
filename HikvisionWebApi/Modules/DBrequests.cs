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
			public string signature { get; set; }
			public object? data { get; set; }
			public List<CameraData> message { get; set; }
		}

		public class CameraData
		{	
			public uint? id { get; set; }
			public bool? active { get; set; }
			public string password { get; set; }
			public ushort? screen_url { get; set; }
			public bool? monitoring { get; set; }
			public bool? mic { get; set; }
			public string rtsp_ip { get; set; }
			public ushort? camera_type { get; set; }
			public ushort? camera_status { get; set; }
			public ushort? settings { get; set; }
			public string detection_mask { get; set; }
			public ushort? detection_status { get; set; }
			public string mac { get; set; }
			public string serial { get; set; }
		}

		public class CamId
		{
			 public uint? id { get; set; }
		}

		public static async Task<string> CameraEdit(uint id, bool audioEnabled)
		{
			var jObject = Requests.ToJObject(await Requests.DeviceInfo());
			var serialNo = (string)jObject["DeviceInfo"]?["serialNumber"];
			var macAddress = (string)jObject["DeviceInfo"]?["macAddress"];
			var data = new DbData
			{
				signature = (string)dbconf?["signature"],
				data = new CameraData
				{
					detection_mask = 
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
					id = id,
					active = true,
					settings = audioEnabled ? 61 : 48,
					camera_status = 105,
					camera_type = 1,
					screen_url = 2,
					detection_status = 205,
					monitoring = true,
					password = (string)dbconf["password"],
					mac = macAddress,
					serial = serialNo,
				}
			};
			JsonContent content = JsonContent.Create(data);
			return await client.PostAsync((string)dbconf["cameraEdit"], content).Result.Content.ReadAsStringAsync();
		}

		public static async Task<List<CameraData>> CameraGet(uint id)
		{
			var data = new DbData()
			{
				signature = (string) dbconf["signature"],
				data = new CamId { id = id }
			};

			JsonContent content = JsonContent.Create(data);
			var response = await client.PostAsync((string) dbconf["cameraGet"], content).Result.Content.ReadAsStringAsync();
			var deserializedResponce = JsonConvert.DeserializeObject<DbData>( response );
			return deserializedResponce.message;
		}
	}
}
