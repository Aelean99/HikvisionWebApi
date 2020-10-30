using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HikvisionWebApi
{
	  public class Web
	  {
			public string ip;
			public string camBaseUri;
			public HttpClient client = new HttpClient();
			public Web(string ip, string password)
			{
				  this.ip = ip;
				  camBaseUri = $"http://{ip}/ISAPI/";
				  //NetworkCredential camCred = new NetworkCredential("admin", password);
				  CredentialCache credCache = new CredentialCache
				  {
						{new UriBuilder(camBaseUri).Uri, "Basic", new NetworkCredential("admin", password)},
						{new UriBuilder(camBaseUri).Uri, "Digest", new NetworkCredential("admin", password)}
				  };

				  client = new HttpClient(
						new SocketsHttpHandler
						{
							  ConnectTimeout = TimeSpan.FromSeconds(30),
							  Credentials = credCache
						}, disposeHandler: true) { BaseAddress = new UriBuilder(camBaseUri).Uri };
			}
	  }

	  [ApiController]
	  public class ISAPIController : ControllerBase
	  {

			/// <summary>
			/// Check pass on admin user
			/// </summary>
			/// <param name="ip">ip address</param>
			/// <param name="password">camera password</param>
			/// <returns>XML</returns>
			[HttpGet]
			[Route("api/[controller]/[action]")]
			public async Task<string> UserCheck(string ip, string password)
			{
				  Web web = new Web(ip, password);
				  var a = await web.client.GetAsync("Security/userCheck");
				  Console.WriteLine(a.RequestMessage);
				  return a.Content.ReadAsStringAsync().Result;
			}
	  }
}
