using System;
using System.Threading.Tasks;
using Hikvision.Modules;
using Microsoft.AspNetCore.Mvc;

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
		public async Task InitClient(string ip)
		{
			try
			{
				await WebClient.InitClient(ip);
			}
			catch (Exception e) { Console.WriteLine(e); throw; }
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
			catch (Exception e) { Console.WriteLine(e); throw; }
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
			catch (Exception e) { Console.WriteLine(e); throw; }
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
			catch (Exception e) { Console.WriteLine(e); throw; }
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
			catch (Exception e) { Console.WriteLine(e); throw; }
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
			catch (Exception a) { Console.WriteLine(a); throw; }
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
			catch (Exception e) { Console.WriteLine(e); throw; }
		}

		/// <summary>
		/// Настройка конфигурации SMTP
		/// </summary>
		/// <param name="smtpServer"></param>
		/// <param name="port"></param>
		/// <returns></returns>
		[HttpPut, Route("[action]")]
		public async Task<string> Email(string smtpServer = "alarm.profintel.ru", int port = 0)
		{
			try
			{
				return await Put.Email(smtpServer, port);
			}
			catch (Exception e) { Console.WriteLine(e); throw; }
		}

		/// <summary>
		/// Настройка NTP
		/// </summary>
		/// <param name="ip"></param>
		/// <param name="addressFormatType">Если ip, то ntp в виде доменного имени не будет работать. Если <c>hostname</c>, то потребуется заполнять поле <c>hostName</c>(Не реализовано)</param>
		/// <returns></returns>
		[HttpPut, Route("[action]")]
		public async Task<string> Ntp(string ip = "217.24.176.232", string addressFormatType = "ip")
		{
			try
			{
				return await Put.Ntp(ip, addressFormatType);
			}
			catch (Exception e) { Console.WriteLine(e); throw; }
		}

		/// <summary>
		/// Настройка часового пояса
		/// </summary>
		/// <param name="timezone">Часовой пояс</param>
		/// <returns></returns>
		[HttpPut, Route("[action]")]
		public async Task<string> Time(string timezone = "CST-5:00:00")
		{
			try
			{
				return await Put.Time(timezone);
			}
			catch (Exception a) { Console.WriteLine(a); throw; }
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
		public async Task<string> StreamConfig(int videoResolutionWidth = 1280,
			int videoResolutionHeight = 720,
			int maxBitrate = 1024,
			string videoCodec = "H.264",
			bool audioEnabled = false,
			string audioCompressType = "MP2L2")
		{
			try
			{
				return await Put.StreamingChannel(videoResolutionWidth, videoResolutionHeight, maxBitrate, videoCodec, audioEnabled, audioCompressType);
			}
			catch (Exception a) { Console.WriteLine(a); throw; }
		}

		/// <summary>
		/// Настройка DNS серверов
		/// </summary>
		/// <returns></returns>
		[HttpPut, Route("[action]")]
		public async Task<string> ChangeDns()
		{
			try
			{
				return await Put.ChangeDns();
			}
			catch (Exception a) { Console.WriteLine(a); throw; }
		}

		/// <summary>
		/// Настройка маски детекции
		/// </summary>
		/// <param name="gridMap">
		/// Сетка детекции. По умолчанию заполняется полностью
		/// Состоит из 22 столбцов и 18 рядов
		/// Каждый ряд группируется по 4 ячейки, в конце остаётся 2
		/// Значение маски в виде строки hexdecimal
		/// Выключенное состояние ячейки: 0
		///Возможные варианты: 1, 2, 4, 8, a(10), b(11), c(12), d(13), e(14), f(15)
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
		public async Task<string> SetDetectionMask(
			string gridMap = "fffffcfffffcfffffcfffffcfffffcfffffcfffffcfffffcfffffcfffffcfffffcfffffcfffffcfffffcfffffcfffffcfffffcfffffc")
		{
			try
			{
				return await Put.SetDetectionMask(gridMap);
			}
			catch (Exception a) { Console.WriteLine(a); throw; }
		}
	}
}
