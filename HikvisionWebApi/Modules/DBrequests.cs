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
