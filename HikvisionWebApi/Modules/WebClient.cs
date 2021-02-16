using Hikvision.RequestsData;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hikvision.Modules
{
	public class WebClient
	{
		public static string CamIp { get; private set; }
		public static int CamStatusCode { get; private set; }
		internal static HttpClient Client { get; private set; }
		internal static string Password { get; private set; }

		internal WebClient( string ip, string password )
		{
			var camBaseUri = $"http://{ip}/ISAPI/";

			CredentialCache credCache = new()
			{
				{ new UriBuilder( camBaseUri ).Uri, "Basic", new NetworkCredential( "admin", password ) },
				{ new UriBuilder( camBaseUri ).Uri, "Digest", new NetworkCredential( "admin", password ) }
			};

			Client = new HttpClient( new SocketsHttpHandler { ConnectTimeout = TimeSpan.FromSeconds( 30 ), Credentials = credCache }, disposeHandler: true )
			{
				BaseAddress = new UriBuilder( camBaseUri ).Uri
			};
		}

		public class PasswordCollection
		{
			[JsonProperty( "passwords" )] public List<string> Passwords { get; set; }
		}

		internal static async Task<int> InitClient( string ip )
		{
			CamIp = ip;
			var passwordCollection = JsonConvert.DeserializeObject<PasswordCollection>( File.ReadAllText( "passwords.json" ) );

			foreach ( var password in passwordCollection.Passwords )
			{
				WebClient wc = new( ip, password );
				var response = await Client.GetAsync( "Security/userCheck" );
				if ( (int) response.StatusCode == 404 ) //Проверяем статус HTTP запроса, если 404 - то устройство не поддерживается и в дальнейших вычислениях нет смысла
				{
					CamStatusCode = (int) response.StatusCode;
					return 404;
				}

				var jsonResponse = Converters.XmlToJson( await response.Content.ReadAsStringAsync() ); // Если запрос прошёл, камера вернёт ответ в виде XML
				var responseData = JsonConvert.DeserializeObject<CamResponses>( jsonResponse );
				CamStatusCode = responseData.userCheck.StatusValue; // Извлекаем результат из тела ответа камеры
				switch ( CamStatusCode )
				{
					case 200:
						{
							Password = passwordCollection.Passwords[0];
							return CamStatusCode;
						} // Если пароль подошёл, то в дальнейшем переборе паролей нет смысла, прерывается цикл
					case 401: { CamStatusCode = 401; break; } // Если пароль не подошёл, продолжаем перебор паролей
					default: { break; }
				}
			}
			return CamStatusCode;
		}
	}
}
