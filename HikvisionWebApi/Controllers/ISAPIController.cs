using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Hikvision.Modules;
using Hikvision.RequestsData;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using static Hikvision.Modules.DBrequests;

using WebClient = Hikvision.Modules.WebClient;

namespace Hikvision.Controllers
{
	static class StringExtensions
	{
		public static IEnumerable<string> Split( this string s, int chunkSize )
		{
			int chunkCount = s.Length / chunkSize;
			for ( int i = 0; i < chunkCount; i++ )
				yield return s.Substring( i * chunkSize, chunkSize );

			if ( chunkSize * chunkCount < s.Length )
				yield return s.Substring( chunkSize * chunkCount );
		}
	}

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
		/// Метод сериализует класс с данными в XML, затем преобразует в StreamContent для отправки в теле запроса.
		/// </summary>
		/// <param name="data">Объект для сериализации</param>
		/// <returns></returns>
		internal static StreamContent SerializeXmlData( object data )
		{
			MemoryStream ms = new();
			new XmlSerializer( data.GetType() ).Serialize( ms, data );
			ms.Seek( 0, SeekOrigin.Begin );
			StreamContent content = new( ms );
			return content;
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
			catch (Exception e) { return e.Message; }
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
			catch (Exception e) { return e.Message; }
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
			catch (Exception e) { return e.Message; }
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
			catch (Exception e) { return e.Message; }
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
			catch (Exception e) { return e.Message; }
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
			catch (Exception e) {return e.Message; }
		}

		
		/// <summary>
		/// SMTP configuration
		/// </summary>
		/// <param name="data">jsonData</param>
		/// <returns></returns>
		[HttpPut, Route( "[action]" )]
		public async Task<string> SetEmailFromBody( [FromBody] EmailData.Mailing data )
		{
			try
			{
				var jObject = Requests.ToJObject( await Requests.DeviceInfo() );
				var serial = $"HK-{jObject["DeviceInfo"]?["serialNumber"]}@camera.ru"; //HK-serialNumber@camera.ru
				data.sender.emailAddress = serial;
				data.receiverList.receiver.emailAddress = serial;

				using var xmlData = ToStringContent( data, "mailing" );
				return await WebClient.Client.PutAsync( "System/Network/mailing/1", xmlData ).Result.Content.ReadAsStringAsync();
			}
			catch ( Exception e ) {return e.Message; }
		}

		/// <summary>
		/// Настраивает NTP конфигурацию
		/// </summary>
		/// <param name="data">jsonData</param>
		/// <returns></returns>
		[HttpPut, Route( "[action]" )]
		public async Task<string> SetNtpFromBody( [FromBody] TimeData.NTPServer data )
		{
			try
			{
				using var xmlData = ToStringContent( data, "NTPServer" );
				return await WebClient.Client.PutAsync( "System/time/NtpServers/1", xmlData ).Result.Content.ReadAsStringAsync();
			}
			catch ( Exception e ) {return e.Message; }
		}


		/// <summary>
		/// Настройка часового пояса
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		[HttpPut, Route( "[action]" )]
		public async Task<string> SetTimeFromBody( [FromBody] TimeData.Time data )
		{
			try
			{
				using var xmlData = ToStringContent( data, "Time" );
				return await WebClient.Client.PutAsync( "System/time", xmlData ).Result.Content.ReadAsStringAsync();
			}
			catch ( Exception e ) { return e.Message; }
		}

		
		/// <summary>
		/// Настройка видео-аудио конфигурации
		/// </summary>
		/// <param name="data">jsonData</param>
		/// <returns></returns>

		[HttpPut, Route( "[action]" )]
		public async Task<string> SetStreamConfigFromBody([FromBody] StreamingData.StreamingChannel data)
		{
			try
			{
				using var xmlData = ToStringContent( data, "StreamingChannel" );
				return await WebClient.Client.PutAsync("Streaming/channels/101", xmlData).Result.Content.ReadAsStringAsync();
			}
			catch ( Exception e ) { return e.Message; }
		}


		/// <summary>
		/// Change DNS on device
		/// </summary>
		/// <param name="data">jsonData</param>
		/// <returns></returns>
		[HttpPut, Route( "[action]" )]
		public async Task<string> SetDnsFromBody([FromBody] NetworkData.RootData data)
		{
			// запрашиваем конфигурацию сети с камеры
			// без этих данных камера не примет XML
			var jObject = Requests.ToJObject( await Requests.Ethernet() );

			//извлекаем текущие значения конфигурации сети
			var addressingType = jObject["IPAddress"]?["addressingType"]?.ToString();

			if ( addressingType == "static" )
			{
				var ipAddress = jObject["IPAddress"]?["ipAddress"]?.ToString();
				var subnetMask = jObject["IPAddress"]?["subnetMask"]?.ToString();
				var defaultGateway = jObject["IPAddress"]?["DefaultGateway"]?["ipAddress"]?.ToString();

				//дополняем пришедший json полученными значениями с камер
				data.ipAddress = ipAddress;
				data.subnetMask = subnetMask;
				data.DefaultGateway.ipAddress = defaultGateway;

				try
				{
					using var xmlData = ToStringContent( data, "IPAddress" );
					return await WebClient.Client.PutAsync( "System/Network/interfaces/1/ipAddress", xmlData ).Result.Content.ReadAsStringAsync();
				}
				catch ( Exception e ) { return e.Message; }
			}
			return $"Addressing type is {addressingType}. Can`t set DNS";
		}


		/// <summary>
		/// Настройка отображения даты и времени на видео-потоке
		/// </summary>
		/// <returns></returns>
		[HttpPut, Route("[action]")]
		public async Task<string> SetOsdChannelNameFromBody( [FromBody] OsdData.channelNameOverlay data)
		{
			try
			{
				using var xmlData = ToStringContent( data, "channelNameOverlay" );
				return await WebClient.Client.PutAsync( "System/Video/inputs/channels/1/overlays/channelNameOverlay", xmlData ).Result.Content.ReadAsStringAsync();
			}
			catch ( Exception e) { return e.Message; }
		}


		/// <summary>
		/// Настройка отображения времени на канале
		/// Отключение отображения
		/// </summary>
		/// <param name="data">jsonData</param>
		/// <returns></returns>
		[HttpPut, Route("[action]")]
		public async Task<string> SetOsdDateTimeFromBody( [FromBody] OsdData.dateTimeOverlay data)
		{
			try
			{
				using var xmlData = ToStringContent( data, "dateTimeOverlay " );
				return await WebClient.Client.PutAsync( "System/Video/inputs/channels/1/overlays/dateTimeOverlay", xmlData ).Result.Content.ReadAsStringAsync();
			}
			catch ( Exception e) { return e.Message; }
		}


		/// <summary>
		/// Настройка способа отправки детекции
		/// </summary>
		/// <param name="data">jsonData</param>
		/// <returns></returns>
		[HttpPut, Route("[action]")]
		public async Task<string> SetAlarmNotificationsFromBody([FromBody] DetectionData.EventTriggerNotificationList data )
		{
			try
			{
				using var xmlData = ToStringContent( data, "EventTriggerNotificationList" );
				return await WebClient.Client.PutAsync( "Event/triggers/VMD-1/notifications", xmlData ).Result.Content.ReadAsStringAsync();
			}
			catch ( Exception e ) { return e.Message; }
		}


		/// <summary>
		/// Включение детекции движения с предустановленной заполенной маской детекции
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		[HttpPut, Route("[action]")]
		public async Task<string> SetDetectionFromBody([FromBody] DetectionData.MotionDetection data )
		{
			try
			{
				using var xmlData = ToStringContent( data, "MotionDetection" );
				return await WebClient.Client.PutAsync( "System/Video/inputs/channels/1/motionDetection", xmlData ).Result.Content.ReadAsStringAsync();
			}
			catch ( Exception e ) { return e.Message; }
		}


		[HttpPut, Route( "[action]" )]
		public async Task<string> ChangeDetectionMaskFromBody( [FromBody] DetectionData.GridFromDB data )
		{
			byte[] hexValues = { 8, 4, 2, 1 }; // Значения для конвертирования входящей маски в понятный для камеры вид 
			StringBuilder gridMap = new();  // объявление итоговой маски для камеры

			try
			{
				var splittedArray = data.dbGridMap.Split( 22 ); // Маска поступает из 396 значений, разделить их на многомерный массив по 22 значения
				int sum = 0; // сумма значений в массиве на основе hexValues
				foreach ( var value in splittedArray ) // Выбрать получившиеся массивы разделённые по 22 значения
				{															   
					foreach ( var value2 in value.Split( 4 ) ) // раделить полученные ранее массивы по 22 элемента - ещё по 4
					{
						for ( int i = 0; i < value2.Length; i++ )
						{
							if ( (int) char.GetNumericValue( value2[i] ) == 1 ) // строку конвертим в charArray, и извлекаем из него int значение. Если пришедшее значение = 1(ячейка активирована)
							{
								sum += hexValues[i]; // ..то извлекаем значение из hexValues и находим сумму значений многомерного массива(4 значения)
							}
							else
							{
								sum += 0; // Если значение 0, то оставляем сумму без изменений
							}

						}
						gridMap.Append( sum.ToString( "x" ) ); // Конвертируем итоговую сумму первого многомерного массива в 16-ричную систему и помещаем в конец StringBuilder
						sum = 0; //сбрасываем результат предыдущих вычислений
					}
				}

				var objData = new DetectionData.MotionDetectionLayout
				{
					sensitivityLevel = 60,
					layout = new DetectionData.Layout
					{
						gridMap = gridMap.ToString()
					}
				};

				using var xmlData = ToStringContent( objData, "MotionDetectionGridLayout" );
				return await WebClient.Client.PutAsync( "System/Video/inputs/channels/1/motionDetection/layout/gridLayout", xmlData ).Result.Content.ReadAsStringAsync();
			}
			catch ( Exception e){ return e.Message; }
		}

		[HttpPost, Route("[action]")]
		public async Task SetAllConfigurations([FromBody] MassConfigData data)
		{
			var camListData = await CameraGet( data.id ); //получить данные о камере из базы 
			string cam_ip = string.Empty;

			foreach ( var item in camListData ) 
			{
				cam_ip = item.rtsp_ip; //извлекаем ip адрес для создания подключения с камерой
			}

			if ( WebClient.Client is null )  //Если клиент не соединён ни с одной из камер - то инициализировать соединие
			{
				await WebClient.InitClient( cam_ip );
			}
			else if(WebClient.cam_ip != cam_ip ) //Так же если у созданного соединения с камерой, старый ip отличается от нового, то создать подключение с новым ip адресом 
			{
				await WebClient.InitClient( cam_ip );
			}
			try
			{
				Console.WriteLine($"Connecting with {cam_ip}");
				Console.WriteLine( $"Time configuration: \n{await SetTimeFromBody( data.timeData )}" );
				Console.WriteLine( $"Ntp configuration: \n{await SetNtpFromBody( data.ntpData )}" );
				Console.WriteLine( $"Osd channel name configuration: \n{await SetOsdChannelNameFromBody( data.osdChannelNameData )}" );
				Console.WriteLine( $"Osd datetime configuration: \n{await SetOsdDateTimeFromBody( data.osdDateTimeData )}" );
				Console.WriteLine( $"Email configuration: \n{await SetEmailFromBody( data.emailData )}" );
				Console.WriteLine( $"Detection configuration: \n{await SetDetectionFromBody( data.detectionData )}" );
				Console.WriteLine( $"Notifications configuration: \n{await SetAlarmNotificationsFromBody( data.eventTriggerData )}" );
				Console.WriteLine( $"Network(dns) configuration: \n{await SetDnsFromBody( data.networkData )}" );
				Console.WriteLine( $"Streaming configuration: \n{await SetStreamConfigFromBody( data.streamingData )}" );
			}
			catch ( Exception e)
			{
				Console.WriteLine( e.Message );
			}
		}
	}
}
