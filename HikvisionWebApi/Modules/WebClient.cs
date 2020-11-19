using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HikvisionWebApi.Modules
{
	  public class WebClient
	  {
			private string camBaseUri;
			private string password = "tvmix333";

			public WebClient() { }
			public WebClient(string ip)
			{
				  camBaseUri = $"http://{ip}/ISAPI/";

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
			public static HttpClient Client { get; set; }
	  }
}
