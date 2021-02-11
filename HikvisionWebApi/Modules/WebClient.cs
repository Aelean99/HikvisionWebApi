using Hikvision.RequestsData;

using Newtonsoft.Json;

using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace Hikvision.Modules
{
	public class WebClient
	{
		public static string CamIp { get; set; }
		public static int CamStatusCode { get; set; }
		internal static HttpClient Client { get; set; }
		
		internal WebClient(string ip, string password)
		{
			var camBaseUri = $"http://{ip}/ISAPI/";
			
			CredentialCache credCache = new()
			{
					{new UriBuilder(camBaseUri).Uri, "Basic", new NetworkCredential("admin", password)},
					{new UriBuilder(camBaseUri).Uri, "Digest", new NetworkCredential("admin", password)}
			}; 
				
			Client = new HttpClient(new SocketsHttpHandler{ConnectTimeout = TimeSpan.FromSeconds(30),Credentials = credCache}, disposeHandler: true)
			{ 
				BaseAddress = new UriBuilder(camBaseUri).Uri 
			};
		}


		internal static async Task<int> InitClient(string ip)
		{
			CamIp = ip;
			StringCollection passwordCollection = new() { "tvmix333", "12345", "admin" };

			foreach (var password in passwordCollection)
			{
				WebClient wc = new(ip, password);
				var response = await Client.GetAsync("Security/userCheck");
				if ( (int)response.StatusCode == 404 ) //Проверяем статус HTTP запроса, если 404 - то устройство не поддерживается и в дальнейших вычислениях нет смысла
				{
					CamStatusCode = (int) response.StatusCode;
					return 404;
				}

				var jsonResponse = Converters.XmlToJson(await response.Content.ReadAsStringAsync()); // Если запрос прошёл, камера вернёт ответ в виде XML
				CamResponses responseData = JsonConvert.DeserializeObject<CamResponses>( jsonResponse );
				CamStatusCode = responseData.userCheck.StatusValue; // Извлекаем результат из тела ответа камеры
				switch ( CamStatusCode )
				{
					case 200: { return CamStatusCode; } // Если пароль подошёл, то в дальнейшем переборе паролей нет смысла, прерывается цикл
					case 401: { CamStatusCode = 401; break; } // Если пароль не подошёл, продолжаем перебор паролей
					default: { break; }
				}
			}
			return CamStatusCode;
		}
	}
}
