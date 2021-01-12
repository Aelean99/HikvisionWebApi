using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;

//using Newtonsoft.Json;

namespace Hikvision.Modules
{
	public class DBrequests
	{
		static JObject dbconf = JObject.Parse( File.ReadAllText( "dbvalues.json" ) );
		public static HttpClient _client = new( new SocketsHttpHandler(), false ) 
		{
			BaseAddress = new Uri((string) dbconf["url"])
		};

		public class DbData
		{
			public string signature { get; set; }
			public object data { get; set; }
		}

		public class Data
		{
			public uint id { get; set; }
			public bool active { get; set; }
			public string password { get; set; }
			public ushort screen_url { get; set; }
			public bool monitoring { get; set; }
			public ushort camera_type { get; set; }
			public ushort camera_status { get; set; }
			public ushort settings { get; set; }
			public string detection_mask { get; set; }
			public ushort detection_status { get; set; }
			public string mac { get; set; }
			public string serial { get; set; }
		}

		public class CameraGetData
		{
			 public uint id { get; set; }
		}

		public static async Task<string> CameraEdit(uint id, bool audioEnabled)
		{
			var jObject = Requests.ToJObject(await Requests.DeviceInfo());
			var serialNo = (string)jObject["DeviceInfo"]?["serialNumber"];
			var macAddress = (string)jObject["DeviceInfo"]?["macAddress"];
			var data = new DbData
			{
				signature = (string)dbconf?["signature"],
				data = new Data
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
			return await _client.PostAsync((string)dbconf["cameraEdit"], content).Result.Content.ReadAsStringAsync();
		}

		public static async Task<JObject> CameraGet(uint id)
		{
			var data = new DbData()
			{
				signature = (string) dbconf["signature"],
				data = new CameraGetData() {id = id}
			};

			JsonContent content = JsonContent.Create(data);
			var response = await _client.PostAsync((string) dbconf["cameraGet"], content).Result.Content.ReadAsStringAsync();
			JObject cameraJobject = JObject.Parse(response);
			return cameraJobject;
		}
	}
}
