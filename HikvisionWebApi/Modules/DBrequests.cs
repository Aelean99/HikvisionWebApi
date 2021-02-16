using Hikvision.RequestsData;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NLog;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Hikvision.Modules
{
	public class DBrequests
	{
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

		private static readonly JObject Dbconf = JObject.Parse( File.ReadAllText( "dbvalues.json" ) );
		private static readonly HttpClient Client = new( new SocketsHttpHandler(), false )
		{
			BaseAddress = new Uri( (string) Dbconf["url"] ?? string.Empty )
		};

		public static async Task<string> CameraEdit( uint id, bool audioEnabled, string rtsp_ip )
		{
			_logger.Info( $"[CameraEdit] Method started" );
			var deviceInfoObject = await GetRequests.DeviceInfo();
			if ( deviceInfoObject is null )
			{
				_logger.Warn( $"[CameraEdit] Empty response from device" );
				return "empty response from device";
			}
			var serialNo = deviceInfoObject.deviceInfo.serialNumber;
			var macAddress = deviceInfoObject.deviceInfo.macAddress;
			var data = new DbData
			{
				Signature = (string) Dbconf?["signature"],
				Data = new CameraData
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
					Id = id,
					Active = true,
					camera_status = 105,
					camera_type = 1,
					screen_url = 2,
					Mic = audioEnabled,
					detection_status = 205,
					Monitoring = true,
					Password = (string) Dbconf["password"],
					Mac = macAddress,
					Serial = serialNo,
					rtsp_ip = rtsp_ip
				}
			};
			JsonContent content = JsonContent.Create( data );
			var response = await Client.PostAsync( (string) Dbconf["cameraEdit"], content ).Result.Content.ReadAsStringAsync();
			_logger.Info( "[CameraEdit] Method completed" );
			return response;
		}

		public static async Task<string> CameraEdit( uint id )
		{
			_logger.Info( "[CameraEdit] Method started" );
			var data = new DbData
			{
				Signature = (string) Dbconf?["signature"],
				Data = new CameraError()
				{
					Id = id,
					camera_status = 104
				}
			};
			JsonContent content = JsonContent.Create( data );
			var response = await Client.PostAsync( (string) Dbconf["cameraEdit"], content ).Result.Content.ReadAsStringAsync();
			_logger.Info( "[CameraEdit] Method completed" );
			return response;
		}

		public static async Task<List<CameraData>> CameraGet( uint id )
		{
			_logger.Info( "[CameraGet] Method started" );
			var data = new DbData()
			{
				Signature = (string) Dbconf["signature"],
				Data = new CamId { Id = id }
			};

			JsonContent content = JsonContent.Create( data );
			var response = await Client.PostAsync( (string) Dbconf["cameraGet"], content ).Result.Content.ReadAsStringAsync();
			var deserializedResponce = JsonConvert.DeserializeObject<DbData>( response );
			_logger.Info( "[CameraGet] Method completed" );
			return deserializedResponce.Message;
		}
	}
}
