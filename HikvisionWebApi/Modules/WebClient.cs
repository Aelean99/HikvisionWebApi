using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hikvision.Modules
{
	  public class WebClient
	  {
		  internal WebClient(string ip, string password)
			{
				var camBaseUri = $"http://{ip}/ISAPI/";

				CredentialCache credCache = new()
				{
					  {new UriBuilder(camBaseUri).Uri, "Basic", new NetworkCredential("admin", password)},
					  {new UriBuilder(camBaseUri).Uri, "Digest", new NetworkCredential("admin", password)}
				}; 
				
				Client = new HttpClient(
						new SocketsHttpHandler
						{
							  ConnectTimeout = TimeSpan.FromSeconds(30),
							  Credentials = credCache
						}, disposeHandler: true)
				  { BaseAddress = new UriBuilder(camBaseUri).Uri };
			}
			internal static HttpClient Client { get; private set; }
			internal static async Task InitClient(string ip)
			{
				StringCollection passwordCollection = new() { "tvmix333", "12345", "admin" };
				foreach (var i in passwordCollection)
				{
					WebClient wc = new(ip, i);
					var response = await Client.GetAsync("Security/userCheck");
					if (response.StatusCode == HttpStatusCode.OK)
					{
						break;
					}
					Console.WriteLine(response.RequestMessage);
					Console.WriteLine(response.StatusCode);
				}
			}

	}

}
