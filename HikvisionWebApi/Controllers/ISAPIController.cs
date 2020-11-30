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
		public async Task<string> Email(string smtpServer = "alarm.profintel.ru", int port = 0)
		{
			return await Put.Email(smtpServer, port);
		}

		[HttpPut, Route("[action]")]
		public async Task<string> Ntp(string ip = "217.24.176.232", string addressFormatType = "ip")
		{
			return await Put.Ntp(ip, addressFormatType);
		}

		[HttpPut, Route("[action]")]
		public async Task<string> Time(string timezone = "CST-5:00:00")
		{
			return await Put.Time(timezone);
		}

		[HttpPut, Route("[action]")]
		public async Task<string> StreamConfig(int videoResolutionWidth = 1280,
			int videoResolutionHeight = 720,
			int maxBitrate = 1024,
			string videoCodec = "H.264",
			bool audioEnabled = false,
			string audioCompressType = "MP2L2")
		{
			return await Put.StreamingChannel(videoResolutionWidth, videoResolutionHeight, maxBitrate, videoCodec, audioEnabled, audioCompressType);
		}

	}
}
