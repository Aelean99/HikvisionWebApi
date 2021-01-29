using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Hikvision.Modules;
using Hikvision.RequestsData;

using static Hikvision.Modules.Put;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Hikvision.Modules.DBrequests;
using WebClient = Hikvision.Modules.WebClient;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Hikvision.Controllers
{ 
	[ApiController, Route("api/[controller]/")]
	public class IsapiController : ControllerBase
	{
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
		/// Настройка конфигурации SMTP
		/// </summary>
		/// <param name="smtpServer"></param>
		/// <param name="port"></param>
		/// <returns></returns>
		[HttpPut, Route("[action]")]
		public async Task<(HttpStatusCode statusCode, string text)> SetEmail(string smtpServer = "alarm.profintel.ru", int port = 0)
		{
			try
			{
				var (statusCode, text) = await Put.Email(smtpServer, port);
				return (statusCode, text);
			}
			catch (Exception e) { Console.WriteLine(e.Message); throw; }
		}

		/// <summary>
		/// Настройка NTP
		/// </summary>
		/// <param name="ip"></param>
		/// <param name="addressFormatType">Если ip, то ntp в виде доменного имени не будет работать. Если <c>hostname</c>, то потребуется заполнять поле <c>hostName</c>(Не реализовано)</param>
		/// <returns></returns>
		//[HttpPut, Route("[action]")]
		//public async Task<(HttpStatusCode statusCode, string text)> Ntp(string ip = "217.24.176.232", string addressFormatType = "ip")
		//{
		//	try
		//	{
		//		var (statusCode, text) = await Put.Ntp();
		//		return (statusCode, text);
		//	}
		//	catch (Exception e) { Console.WriteLine(e.Message); throw; }
		//}

		//[HttpPost, Route( "[action]" )]
		//public async Task<string> NtpFromBody( [FromBody] TimeData.NTPServer data )
		//{
		//	try
		//	{
		//		var jsonData = JsonConvert.SerializeObject( data );
		//		XNode node = JsonConvert.DeserializeXNode( jsonData , "NTPServer" );
		//		Console.WriteLine($"Node: {node}");
		//		HttpContent content = new StringContent( node );
		//		var response = await WebClient.Client.PutAsync( "System/time/NtpServers", content );
		//		//var (statusCode, text) = await Put.Ntp(data);
		//		//return (statusCode, text);
		//		return await response.Content.ReadAsStringAsync();
		//	}
		//	catch ( Exception e ) { Console.WriteLine( e.Message ); throw; }
		//}

		/// <summary>
		/// Настройка часового пояса
		/// </summary>
		/// <param name="timezone">Часовой пояс</param>
		/// <returns></returns>
		[HttpPut, Route("[action]")]
		public async Task<(HttpStatusCode statusCode, string text)> SetTime(string timezone = "CST-5:00:00")
		{
			try
			{
				var (statusCode, text) = await Put.Time(timezone);
				return (statusCode, text);
			}
			catch (Exception a) { Console.WriteLine(a.Message); throw; }
		}

		/// <summary>
		/// Настройка конфигурации качества видео-потока
		/// </summary>
		/// <param name="videoResolutionWidth">Невозможно задать значения отсутствующие в параметрах устройства. Варианты разрешения можно узнать методом capabillities(не реализовано)</param>
		/// <param name="videoResolutionHeight">Невозможно задать значения отсутствующие в параметрах устройства. Варианты разрешения можно узнать методом capabillities(не реализовано)</param>
		/// <param name="maxBitrate">Максимальная скорость которую займёт устройство</param>
		/// <param name="videoCodec"></param>
		/// <param name="audioEnabled">Если true - устройство будет записывать звук</param>
		/// <param name="audioCompressType">MP2L2 - лучший кодек, остальные смотреть в capabillities(не реализован)</param>
		/// <returns></returns>
		[HttpPut, Route("[action]")]
		public async Task<(HttpStatusCode statusCode, string text)> StreamConfig(int videoResolutionWidth = 1280,
			int videoResolutionHeight = 720,
			int maxBitrate = 1024,
			string videoCodec = "H.264",
			bool audioEnabled = false,
			string audioCompressType = "MP2L2")
		{
			try
			{
				var (statusCode, text) = await Put.StreamingChannel(videoResolutionWidth, videoResolutionHeight, maxBitrate, videoCodec, audioEnabled, audioCompressType);
				return (statusCode, text);
			}
			catch (Exception a) { Console.WriteLine(a.Message); throw; }
		}

		/// <summary>
		/// Настройка DNS серверов
		/// </summary>
		/// <returns></returns>
		[HttpPut, Route("[action]")]
		public async Task<(HttpStatusCode statusCode, string text)> ChangeDns()
		{
			try
			{
				var (statusCode, text) = await Put.ChangeDns();
				return (statusCode, text);
			}
			catch (Exception a) { Console.WriteLine(a.Message); throw; }
		}

		/// <summary>
		/// Настройка маски детекции
		/// </summary>
		/// <param name="gridMap">
		/// Сетка детекции. По умолчанию заполняется полностью
		/// Состоит из 22 столбцов и 18 рядов
		/// В каждом ряду ячейки группируются по 4 шт(всего их 22 = количеству стобцов), в конце остаётся 2
		/// Значение маски в виде строки hexdecimal
		/// Выключенное состояние ячейки: 0
		/// Возможные варианты: 1, 2, 4, 8, a(10), b(11), c(12), d(13), e(14), f(15)
		///
		/// Только первая ячейка: 8
		/// Только вторая ячейка: 4
		/// Только третяя ячейка: 2
		/// Только четвёртая ячейка: 1
		/// В зависимости от выбранных ячеек, производится сумма их значений и выдаётся результат
		/// Пример: выбрать 3 и 2 ячейки, их сумма = 6, сетка будет выглядеть так: 6000
		/// Пример: выбрать 1 и 3 ячейки, их сумма = 10, сетка будет выглядеть так: a000
		/// Пример: выбрать 1 2 3 и 4 ячейки, их сумма = 15, сетка будет выглядеть так: f000
		/// Пример: выбрать первые 20 ячеек в первой строке, сетка будет выглядеть так: fffff
		/// Пример: выбрать все 22 ячейки в первой строке, сетка будет выглядеть так: fffffc 
		/// </param>
		/// <returns></returns>
		[HttpPut, Route("[action]")]
		public async Task<(HttpStatusCode statusCode, string text)> SetDetectionMask(
			string gridMap = "fffffcfffffcfffffcfffffcfffffcfffffcfffffcfffffcfffffcfffffcfffffcfffffcfffffcfffffcfffffcfffffcfffffcfffffc")
		{
			try
			{
				var (statusCode, text) = await Put.SetDetectionMask(gridMap);
				return (statusCode, text);
			}
			catch (Exception a) { Console.WriteLine(a.Message); throw; }
		}

		/// <summary>
		/// Настройка отображения даты и времени на видео-потоке
		/// </summary>
		/// <returns></returns>
		[HttpPut, Route("[action]")]
		public async Task<(HttpStatusCode statusCode, string text)> OsdDateTime()
		{
			var (statusCode, text) = await Put.OsdDateTime();
			return (statusCode, text);
		}


		/// <summary>
		/// Настройка отображения имени канала/устройства на видео-потоке
		/// Отключение отображения
		/// </summary>
		/// <returns></returns>
		[HttpPut, Route("[action]")]
		public async Task<(HttpStatusCode statusCode, string text)> OsdChannelName()
		{
			var (statusCode, text) = await Put.OsdChannelName(); 
			return (statusCode, text);
		}

		[HttpGet, Route( "[action]" )]
		public async Task SetAllConfigurations( uint id, bool audioEnabled )
		{
			var cameraObj = await DBrequests.CameraGet( id );
			var result = cameraObj?["result"];
			JObject cameraData = (JObject) JsonConvert.DeserializeObject( string.Join( "", result ) );
			string ip = cameraData["rtsp_ip"].ToString();

			var connectionStatus = await InitClient( ip );
			if ( connectionStatus == HttpStatusCode.OK )
			{
				Console.WriteLine( $"Connection initializing.. {connectionStatus}" );
				Console.WriteLine( $"NTP configuring: {await Ntp().ContinueWith( antecedent => antecedent.Result.statusCode )}" );
				Console.WriteLine( $"Time configuring: {await SetTime().ContinueWith( antecedent => antecedent.Result.statusCode )}" );
				Console.WriteLine( $"VideoStream configuring: {await StreamConfig( audioEnabled: audioEnabled ).ContinueWith( antecedent => antecedent.Result.statusCode )}" );
				Console.WriteLine( $"Email configuring: {await SetEmail().ContinueWith( antecedent => antecedent.Result.statusCode )}" );
				Console.WriteLine( $"Dns configuring: {await ChangeDns().ContinueWith( antecedent => antecedent.Result.statusCode )}" );
				Console.WriteLine( $"DetectionMask configuring: {await SetDetectionMask().ContinueWith( antecedent => antecedent.Result.statusCode )}" );
				Console.WriteLine( $"OsdChannelName configuring: {await OsdChannelName().ContinueWith( antecedent => antecedent.Result.statusCode )}" );
				Console.WriteLine( $"OsdDateTime configuring: {await OsdDateTime().ContinueWith( antecedent => antecedent.Result.statusCode )}" );
				Console.WriteLine( $"DBedit status: {await DBrequests.CameraEdit( id, audioEnabled )}" );
			}
			else
			{
				Console.WriteLine( $"Connection initializing.. {connectionStatus}" );
			}
		}

		[HttpPost, Route( "[action]" )]
		public async Task SetAllConfigurationsFromBody( [FromBody] CameraData camData )
		{
			var connectionStatus = await InitClient( camData.rtsp_ip );
			if ( connectionStatus == HttpStatusCode.OK )
			{
				Console.WriteLine( $"Connection initializing.. {connectionStatus}" );
				Console.WriteLine( $"NTP configuring: {await Ntp().ContinueWith( antecedent => antecedent.Result.statusCode )}" );
				Console.WriteLine( $"Time configuring: {await SetTime().ContinueWith( antecedent => antecedent.Result.statusCode )}" );
				Console.WriteLine( $"VideoStream configuring: {await StreamConfig( audioEnabled: camData.mic ).ContinueWith( antecedent => antecedent.Result.statusCode )}" );
				Console.WriteLine( $"Email configuring: {await SetEmail().ContinueWith( antecedent => antecedent.Result.statusCode )}" );
				Console.WriteLine( $"Dns configuring: {await ChangeDns().ContinueWith( antecedent => antecedent.Result.statusCode )}" );
				Console.WriteLine( $"DetectionMask configuring: {await SetDetectionMask().ContinueWith( antecedent => antecedent.Result.statusCode )}" );
				Console.WriteLine( $"OsdChannelName configuring: {await OsdChannelName().ContinueWith( antecedent => antecedent.Result.statusCode )}" );
				Console.WriteLine( $"OsdDateTime configuring: {await OsdDateTime().ContinueWith( antecedent => antecedent.Result.statusCode )}" );
				Console.WriteLine( $"DBedit status: {await DBrequests.CameraEdit( camData.id, camData.mic )}" );
			}
			else
			{
				Console.WriteLine( $"Connection initializing.. {connectionStatus}" );
			}
		}
	}
}
