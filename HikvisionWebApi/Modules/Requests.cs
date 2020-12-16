using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Hikvision.RequestsData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hikvision.Modules
{
	internal static class Requests
	{
		/// <summary>
		/// Метод преобразует XML ответ устройства в jObject для взаимодействия с возможностями json
		/// </summary>
		/// <param name="data">XML объект из которого в дальнейшем будут извлечены значения</param>
		/// <returns>десериализованный jobject объект</returns>
		internal static JObject ToJObject(object data)
		{
			try
			{
				XmlDocument doc = new();
				doc.LoadXml(data.ToString() ?? string.Empty);
				var jsonContent = JsonConvert.SerializeXmlNode(doc);
				return  (JObject)JsonConvert.DeserializeObject(jsonContent);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		/// <summary>
		/// Получить системную конфигурацию устройства(MAC адрес, serial номер и т.п)
		/// </summary>
		/// <returns></returns>
		public static async Task<string> DeviceInfo()
		{
			return await WebClient.Client.GetStringAsync("System/deviceInfo");
		}

		/// <summary>
		/// Получить конфигурацию настроек времени
		/// </summary>
		/// <returns></returns>
		public static async Task<string> Time()
		{
			return await WebClient.Client.GetStringAsync("System/time");
		}

		/// <summary>
		/// Получить кофнигурацию сетевых интерфейсов
		/// </summary>
		/// <returns></returns>
		public static async Task<string> Ethernet()
		{
			return await WebClient.Client.GetStringAsync("System/Network/interfaces/1/ipAddress");
		}                     

		/// <summary>
		/// Получить конфигурацию SMTP
		/// </summary>
		/// <returns></returns>
		public static async Task<string> Email()
		{
			return await WebClient.Client.GetStringAsync("System/Network/mailing");
		}

		/// <summary>
		/// Получить конфигурацию детекции движения
		/// </summary>
		/// <returns></returns>
		public static async Task<string> Detection()
		{
			return await WebClient.Client.GetStringAsync("System/Video/inputs/channels/1/motionDetection");
		}

		/// <summary>
		/// Сканировать список wi-fi сетей
		/// </summary>
		/// <returns></returns>
		public static async Task<string> Wifi_List()
		{
			return await WebClient.Client.GetStringAsync("System/Network/interfaces/2/wireless/accessPointList");
		}

		/// <summary>
		/// Текущее отображение даты и времени на видео-потоке
		/// </summary>
		/// <returns></returns>
		public static async Task<string> OsdDateTime()
		{
			return await WebClient.Client.GetStringAsync("System/Video/inputs/channels/1/overlays/dateTimeOverlay");
		}


		/// <summary>
		/// Текущее отображение имени устройства/канала на видео-потоке
		/// </summary>
		/// <returns></returns>
		public static async Task<string> OsdChannelName()
		{
			return await WebClient.Client.GetStringAsync(
				"System/Video/inputs/channels/1/overlays/channelNameOverlay");
		}
	}






	internal static class Put
	{
		/// <summary>
		/// Метод сериализует класс с данными в XML, затем преобразует в StreamContent для отправки в теле запроса.
		/// </summary>
		/// <param name="data">Объект для сериализации</param>
		/// <returns></returns>
		private static StreamContent SerializeXmlData(object data)
		{
			MemoryStream ms = new();
			new XmlSerializer(data.GetType()).Serialize(ms, data);
			ms.Seek(0, SeekOrigin.Begin);
			StreamContent content = new(ms);
			return content;
		}

		/// <summary>
		/// Настройка smtp конфигурации
		/// </summary>
		/// <param name="smtpServer"></param>
		/// <param name="port"></param>
		/// <returns></returns>
		public static async Task<(HttpStatusCode statusCode, string text)> Email(string smtpServer, int port)
		{
			//SMTP сервер пока-что работает на двух портах.
			//Если порт явно не передан в запросе - выбрать рандомно порт сервера
			if (port is 0)
				port = new Random().Next(15005, 15007);

			//Чтобы обеспечить уникальность email адреса с которого будут посылаться сообщения,
			//необходимо для каждого устройства извлекать серийный номер, и на его основе собирать email адрес отправителя
			var jObject = Requests.ToJObject(await Requests.DeviceInfo());
			var serial = $"HK-{jObject["DeviceInfo"]?["serialNumber"]}@camera.ru"; //HK-serialNumber@camera.ru

			//Данные которые будут преобразованы в XML для отправки в теле запроса
			var data = new EmailData.mailing
			{
				Sender = new EmailData.Sender { EmailAddress = serial, Smtp = new EmailData.Smtp { HostName = smtpServer, PortNo = port} },
				Attachment = new EmailData.Attachment { Snapshot = new EmailData.Snapshot() },
				ReceiverList = new EmailData.ReceiverList { Receiver = new EmailData.Receiver { EmailAddress = serial }}
			};
			//Console.WriteLine(serial);
			using var content = SerializeXmlData(data);
			var response = await WebClient.Client.PutAsync("System/Network/mailing/1", content);
			return (response.StatusCode, await response.Content.ReadAsStringAsync());
		}

		/// <summary>
		/// Настройка NTP на устройстве
		/// </summary>
		/// <param name="ip"></param>
		/// <param name="addressFormatType"></param>
		/// <returns></returns>
		public static async Task<(HttpStatusCode statusCode, string text)> Ntp(string ip, string addressFormatType)
		{
			//Данные которые будут преобразованы в XML для отправки в теле запроса
			var data = new TimeData.NTPServer()
			{
				IpAddress = ip, 
				AddressingFormatType = addressFormatType
			};
			using var content = SerializeXmlData(data);
			var response = await WebClient.Client.PutAsync("System/time/NtpServers", content);
			return (response.StatusCode, await response.Content.ReadAsStringAsync());
		}

		public static async Task<(HttpStatusCode statusCode, string text)> Time(string timezone)
		{
			//Данные которые будут преобразованы в XML для отправки в теле запроса
			var data = new TimeData.Time { TimeZone = timezone };
			using var content = SerializeXmlData(data);
			var response = await WebClient.Client.PutAsync("System/time", content);
			return (response.StatusCode, await response.Content.ReadAsStringAsync());
		}

		public static async Task<(HttpStatusCode statusCode, string text)> StreamingChannel(
			int videoResolutionWidth, 
			int videoResolutionHeight, 
			int maxBitrate, 
			string videoCodec, 
			bool audioEnabled, 
			string audioCompressType)
		{
			//Данные которые будут преобразованы в XML для отправки в теле запроса
			var data = new StreamingData.StreamingChannel
			{
				Audio = new StreamingData.Audio {AudioCompressionType = audioCompressType, Enabled = audioEnabled},
				Video = new StreamingData.Video
				{
					VideoCodecType = videoCodec,
					VbrUpperCap = maxBitrate,
					VideoResolutionHeight = videoResolutionHeight,
					VideoResolutionWidth = videoResolutionWidth
				}
			};
			using var content = SerializeXmlData(data);
			var response = await WebClient.Client.PutAsync("Streaming/channels/101", content);
			return (response.StatusCode, await response.Content.ReadAsStringAsync());
		}

		private static async Task<(HttpStatusCode statusCode, string text)> EnableSendingDetectionToMail()
		{
			//Данные которые будут преобразованы в XML для отправки в теле запроса
			var data = new DetectionData.EventTriggerNotificationList { EventTriggerNotification = new DetectionData.EventTriggerNotification()};
			using var content = SerializeXmlData(data);
			var response = await WebClient.Client.PutAsync("Event/triggers/VMD-1/notifications", content);
			return (response.StatusCode, await response.Content.ReadAsStringAsync());
		}

		/// <summary>
		/// Метод смены DNS
		/// </summary>
		/// <returns></returns>
		public static async Task<(HttpStatusCode statusCode, string text)> ChangeDns()
		{
			var jObject = Requests.ToJObject( await Requests.Ethernet());

			//извлекаем текущие значения конфигурации сети
			var ipAddress = jObject["IPAddress"]?["ipAddress"]?.ToString();
			var subnetMask = jObject["IPAddress"]?["subnetMask"]?.ToString();
			var defaultGateway = jObject["IPAddress"]?["DefaultGateway"]?["ipAddress"]?.ToString();
			var primaryDns = "217.24.176.230";
			var secondaryDns = "217.24.177.2";

			//Данные которые будут преобразованы в XML для отправки в теле запроса
			var data = new NetworkData.IPAddress
			{
				IpAddress = ipAddress,
				SubnetMask = subnetMask,
				DefaultGateway = new NetworkData.DefaultGateway { IpAddress = defaultGateway },
				PrimaryDns = new NetworkData.PrimaryDNS { IpAddress = primaryDns },
				SecondaryDns = new NetworkData.SecondaryDNS {IpAddress = secondaryDns},
				Ipv6Mode = new NetworkData.Ipv6Mode { Ipv6AddressList = new NetworkData.Ipv6AddressList { V6Address = new NetworkData.V6Address() } }
			};

			using var content = SerializeXmlData(data);
			var response = await WebClient.Client.PutAsync("System/Network/interfaces/1/ipAddress", content);
			return (response.StatusCode, await response.Content.ReadAsStringAsync());
		}

		/// <summary>
		/// Настройка маски детекции на устройство
		/// </summary>
		/// <param name="gridMap">Сетка детекции</param>
		/// <returns></returns>
		public static async Task<(HttpStatusCode statusCode, string text)> SetDetectionMask(string gridMap)
		{
			await EnableSendingDetectionToMail();

			//Данные которые будут преобразованы в XML для отправки в теле запроса
			var data = new DetectionData.MotionDetection()
			{
				Enabled = true,
				EnableHighlight = false,
				Grid = new DetectionData.Grid(),
				MotionDetectionLayout = new DetectionData.MotionDetectionLayout
				{
					SensitivityLevel = 60,
					Layout = new DetectionData.Layout {GridMap = gridMap}
				}
			};

			using var content = SerializeXmlData(data);
			var response =
				await WebClient.Client.PutAsync("System/Video/inputs/channels/1/motionDetection", content);
			return (response.StatusCode, await response.Content.ReadAsStringAsync());
		}

		/// <summary>
		/// Настройка отображения даты и времени на видео-потоке
		/// </summary>
		/// <returns></returns>
		public static async Task<(HttpStatusCode statusCode, string text)> OsdDateTime()
		{
			var data = new OsdData.dateTimeOverlay();
			using var content = SerializeXmlData(data);
			var response = await WebClient.Client.PutAsync("System/Video/inputs/channels/1/overlays/dateTimeOverlay",
				content);
			return (response.StatusCode, await response.Content.ReadAsStringAsync());
		}

		/// <summary>
		/// Настройка отображения имени канала на видео-потоке
		/// Выключает отображение
		/// </summary>
		/// <returns></returns>
		public static async Task<(HttpStatusCode statusCode, string text)> OsdChannelName()
		{
			var data = new OsdData.channelNameOverlay();
			using var content = SerializeXmlData(data);
			var response =
				await WebClient.Client.PutAsync("System/Video/inputs/channels/1/overlays/channelNameOverlay",
					content);
			return (response.StatusCode, await response.Content.ReadAsStringAsync());
		}
	}
}
