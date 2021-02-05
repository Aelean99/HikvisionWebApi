using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hikvision.Modules
{
	public class WebClient
	{
		public static string cam_ip { get; set; }
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

		internal static async Task<HttpStatusCode> InitClient(string ip)
		{
			cam_ip = ip;
			StringCollection passwordCollection = new() { "tvmix333", "12345", "admin" };
			HttpStatusCode statusCode = HttpStatusCode.Unused;
			foreach (var password in passwordCollection)
			{
				WebClient wc = new(ip, password);
				var response = await Client.GetAsync("Security/userCheck");
				if (response.StatusCode == HttpStatusCode.OK)
				{
					statusCode = response.StatusCode;
					break;
				}
				else
				{
					statusCode = response.StatusCode;
				}
			}
			return statusCode;
		}

	}

}
