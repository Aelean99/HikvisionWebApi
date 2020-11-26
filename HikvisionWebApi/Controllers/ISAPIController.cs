using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Hikvision.Modules;
using Microsoft.AspNetCore.Mvc;

namespace Hikvision.Controllers
{ 
	[ApiController, Route("api/[controller]/")]
	public class IsapiController : ControllerBase
	{ 
		[HttpGet, Route("[action]")]
		public async Task InitClient(string ip)
		{
			await WebClient.InitClient(ip);
		}

		[HttpGet, Route("[action]")]
		public async Task<string> DeviceInfo()
		{
			return await Requests.DeviceInfo();
		}

		[HttpGet, Route("[action]")]
		public async Task<string> Ethernet()
		{
			return await Requests.Ethernet();
		}

		[HttpGet, Route("[action]")]
		public async Task<string> Time()
		{
			return await Requests.Time();
		}

		[HttpGet, Route("[action]")]
		public async Task<string> Email()
		{
			return await Requests.Email();
		}

		[HttpGet, Route("[action]")]
		public async Task<string> Detection()
		{
			return await Requests.Detection();
		}

		[HttpGet, Route("[action]")]
		public async Task<string> Wifi()
		{
			return await Requests.Wifi_List();
		}

		[HttpPut, Route("[action]")]
		public async Task<string> Email(object _)
		{
			return await Put.Email();
		}
	}
}
