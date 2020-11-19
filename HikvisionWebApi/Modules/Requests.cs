using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Threading.Tasks;

namespace HikvisionWebApi.Modules
{
	  public class Requests : WebClient
	  {
			public static WebClient wc;
			public static async Task<string> UserCheck(string ip)
			{
				  wc = new(ip);
				  var response = await Client.GetAsync("Security/userCheck");
				  Console.WriteLine(response.RequestMessage);
				  return response.Content.ReadAsStringAsync().Result;
			}

			public static async Task<string> DeviceInfo()
			{
				  var response = await Client.GetAsync("System/deviceInfo");
				  return response.Content.ReadAsStringAsync().Result;
			}
	  }
}
