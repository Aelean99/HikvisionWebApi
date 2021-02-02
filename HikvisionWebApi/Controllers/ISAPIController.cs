using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Hikvision.Modules;
using Hikvision.RequestsData;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebClient = Hikvision.Modules.WebClient;

namespace Hikvision.Controllers
{
	[ApiController, Route("api/[controller]/")]
	public class IsapiController : ControllerBase
	{

		private static StringContent ToStringContent(object data, string rootName)
		{
			var jsonData = JsonConvert.SerializeObject( data ); //data to jsonObject
			var xmlString = JsonConvert.DeserializeXNode( jsonData, rootName ).ToString();	// jsonObject to xmlString
			StringContent xmlData = new( xmlString );	// xmlString to StringContent(http content put/post methods)
			return xmlData;
		}

		/// <summary>
		/// Инициализация объекта авторизации на устройство.
		/// 1 раз применить и все последующие методы будут использовать готовый объект с авторизацией, без необходимости каждый раз дёргать метод снова
		/// </summary>
		/// <param name="ip">ip адрес устройства</param>
		/// <returns></returns>
		[HttpGet, Route("[action]")]
		public async Task<HttpStatusCode> InitClient(string ip)
		{
			try
			{
				return await WebClient.InitClient(ip);
			}
			catch (Exception e) { Console.WriteLine(e.Message); throw; }
		}

		/// <summary>
		/// Запросить системную информацию с устойства
		/// </summary>
		/// <returns>Конфигурация в виде строки</returns>
		[HttpGet, Route("[action]")]
		public async Task<string> DeviceInfo()
		{
			try
			{
				return await Requests.DeviceInfo();
			}
			catch (Exception e) { Console.WriteLine(e.Message); throw; }
		}

		/// <summary>
		/// Конфигурация сетевых адаптеров
		/// </summary>
		/// <returns>Конфигурация в виде строки</returns>
		[HttpGet, Route("[action]")]
		public async Task<string> Ethernet()
		{
			try
			{
				return await Requests.Ethernet();
			}
			catch (Exception e) { Console.WriteLine(e.Message); throw; }
		}

		/// <summary>
		/// Конфигурация настроек времени
		/// </summary>
		/// <returns>Конфигурация в виде строки</returns>
		[HttpGet, Route("[action]")]
		public async Task<string> Time()
		{
			try
			{
				return await Requests.Time();
			}
			catch (Exception e) { Console.WriteLine(e.Message); throw; }
		}

		/// <summary>
		/// Конфигурация SMTP протокола
		/// </summary>
		/// <returns>Конфигурация в виде строки</returns>
		[HttpGet, Route("[action]")]
		public async Task<string> Email()
		{
			try
			{
				return await Requests.Email();
			}
			catch (Exception e) { Console.WriteLine(e.Message); throw; }
		}

		/// <summary>
		/// Конфигурация детекции движения на устройстве
		/// </summary>
		/// <returns>Конфигурация в виде строки</returns>
		[HttpGet, Route("[action]")]
		public async Task<string> Detection()
		{
			try
			{
				return await Requests.Detection();
			}
			catch (Exception a) { Console.WriteLine(a.Message); throw; }
		}

		/// <summary>
		/// Список wi-fi сетей которые просканировало устройство
		/// </summary>
		/// <returns>Список сетей в виде строки</returns>
		[HttpGet, Route("[action]")]
		public async Task<string> Wifi()
		{
			try
			{
				return await Requests.Wifi_List();
			}
			catch (Exception e) { Console.WriteLine(e.Message); throw; }
		}

		
		/// <summary>
		/// SMTP configuration
		/// </summary>
		/// <param name="data">jsonData</param>
		/// <returns></returns>
		[HttpPut, Route( "[action]" )]
		public async Task<HttpStatusCode> SetEmailFromBody( [FromBody] EmailData.Mailing data )
		{
			try
			{
				var jObject = Requests.ToJObject( await Requests.DeviceInfo() );
				var serial = $"HK-{jObject["DeviceInfo"]?["serialNumber"]}@camera.ru"; //HK-serialNumber@camera.ru
				data.sender.emailAddress = serial;
				data.receiverList.receiver.emailAddress = serial;

				using var xmlData = ToStringContent( data, "mailing" );
				var response = await WebClient.Client.PutAsync( "System/Network/mailing/1", xmlData );
				return response.StatusCode;
			}
			catch ( Exception e ) { Console.WriteLine( e.Message ); throw; }
		}

		/// <summary>
		/// Настраивает NTP конфигурацию
		/// </summary>
		/// <param name="data">jsonData</param>
		/// <returns></returns>
		[HttpPost, Route( "[action]" )]
		public async Task<HttpStatusCode> SetNtpFromBody( [FromBody] TimeData.NTPServer data )
		{
			try
			{
				using var xmlData = ToStringContent( data, "NTPServer" );
				var response = await WebClient.Client.PostAsync( "System/time/NtpServers", xmlData );
				return response.StatusCode;
			}
			catch ( Exception e ) { Console.WriteLine( e.Message ); throw; }
		}


		/// <summary>
		/// Настройка часового пояса
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		[HttpPut, Route( "[action]" )]
		public async Task<HttpStatusCode> SetTimeFromBody( [FromBody] TimeData.Time data )
		{
			try
			{
				using var xmlData = ToStringContent( data, "Time" );
				var response = await WebClient.Client.PutAsync( "System/time", xmlData );
				return response.StatusCode;

			}
			catch ( Exception a ) { Console.WriteLine( a.Message ); throw; }
		}

		
		/// <summary>
		/// Настройка видео-аудио конфигурации
		/// </summary>
		/// <param name="data">jsonData</param>
		/// <returns></returns>

		[HttpPut, Route( "[action]" )]
		public async Task<HttpStatusCode> SetStreamConfigFromBody([FromBody] StreamingData.StreamingChannel data)
		{
			try
			{
				using var xmlData = ToStringContent( data, "StreamingChannel" );
				var response = await WebClient.Client.PutAsync("Streaming/channels/101", xmlData);
				return response.StatusCode;
			}
			catch ( Exception a ) { Console.WriteLine( a.Message ); throw; }
		}


		/// <summary>
		/// Change DNS on device
		/// </summary>
		/// <param name="data">jsonData</param>
		/// <returns></returns>
		[HttpPut, Route( "[action]" )]
		public async Task<HttpStatusCode> SetDnsFromBody([FromBody] NetworkData.BodyData data)
		{
			try
			{
				using var xmlData = ToStringContent(data, "IPAddress" );
				var response = await WebClient.Client.PutAsync( "System/Network/interfaces/1/ipAddress", xmlData );
				return response.StatusCode;
			}
			catch ( Exception a ) { Console.WriteLine( a.Message ); throw; }
		}


		/// <summary>
		/// Настройка отображения даты и времени на видео-потоке
		/// </summary>
		/// <returns></returns>
		[HttpPut, Route("[action]")]
		public async Task<HttpStatusCode> SetOsdChannelNameFromBody( [FromBody] OsdData.channelNameOverlay data)
		{
			using var xmlData = ToStringContent( data, "channelNameOverlay" );
			var response = await WebClient.Client.PutAsync( "System/Video/inputs/channels/1/overlays/channelNameOverlay", xmlData);
			return response.StatusCode;
		}


		/// <summary>
		/// Настройка отображения времени на канале
		/// Отключение отображения
		/// </summary>
		/// <param name="data">jsonData</param>
		/// <returns></returns>
		[HttpPut, Route("[action]")]
		public async Task<HttpStatusCode> SetOsdDateTimeFromBody( [FromBody] OsdData.dateTimeOverlay data)
		{
			using var xmlData = ToStringContent( data, "dateTimeOverlay " );
			var response = await WebClient.Client.PutAsync( "System/Video/inputs/channels/1/overlays/dateTimeOverlay", xmlData );
			return response.StatusCode;
		}


		/// <summary>
		/// Настройка способа отправки детекции
		/// </summary>
		/// <param name="data">jsonData</param>
		/// <returns></returns>
		[HttpPut, Route("[action]")]
		public async Task<HttpStatusCode> SetAlarmNotificationsFromBody([FromBody] DetectionData.EventTriggerNotificationList data )
		{
			using var xmlData = ToStringContent( data, "EventTriggerNotificationList" );
			var response = await WebClient.Client.PutAsync( "Event/triggers/VMD-1/notifications", xmlData );
			return response.StatusCode;
		}


		/// <summary>
		/// Включение детекции движения с предустановленной заполенной маской детекции
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		[HttpPut, Route("[action]")]
		public async Task<HttpStatusCode> SetDetectionFromBody([FromBody] DetectionData.MotionDetection data )
		{
			using var xmlData = ToStringContent( data, "MotionDetection" );
			var response = await WebClient.Client.PutAsync("System/Video/inputs/channels/1/motionDetection", xmlData);
			return response.StatusCode;
		}


		/// <summary>
		/// Метод смены маски детекции
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		[HttpPut, Route( "[action]" )]
		public async Task<HttpStatusCode> ChangeDetectionMaskFromBody( [FromBody] DetectionData.MotionDetectionLayout data )
		{
			if(WebClient.Client is null )
			{
				await WebClient.InitClient("192.168.0.27");
			}
			using var xmlData = ToStringContent( data, "MotionDetectionGridLayout" );
			var response = await WebClient.Client.PutAsync( "System/Video/inputs/channels/1/motionDetection/layout/gridLayout", xmlData );
			return response.StatusCode;
		}

	}
}
