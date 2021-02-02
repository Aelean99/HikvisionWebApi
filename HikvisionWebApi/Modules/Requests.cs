using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Hikvision.RequestsData;

using Microsoft.AspNetCore.Mvc;

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
				Console.WriteLine(e.Message);
				throw;
			}
		}

		/// <summary>
		/// Получить системную конфигурацию устройства(MAC адрес, serial номер и т.п)
		/// </summary>
		/// <returns></returns>
		internal static async Task<string> DeviceInfo()
		{
			return await WebClient.Client.GetStringAsync("System/deviceInfo");
		}

		/// <summary>
		/// Получить конфигурацию настроек времени
		/// </summary>
		/// <returns></returns>
		internal static async Task<string> Time()
		{
			return await WebClient.Client.GetStringAsync("System/time");
		}

		/// <summary>
		/// Получить кофнигурацию сетевых интерфейсов
		/// </summary>
		/// <returns></returns>
		internal static async Task<string> Ethernet()
		{
			return await WebClient.Client.GetStringAsync("System/Network/interfaces/1/ipAddress");
		}                     

		/// <summary>
		/// Получить конфигурацию SMTP
		/// </summary>
		/// <returns></returns>
		internal static async Task<string> Email()
		{
			return await WebClient.Client.GetStringAsync("System/Network/mailing");
		}

		/// <summary>
		/// Получить конфигурацию детекции движения
		/// </summary>
		/// <returns></returns>
		internal static async Task<string> Detection()
		{
			return await WebClient.Client.GetStringAsync("System/Video/inputs/channels/1/motionDetection");
		}

		/// <summary>
		/// Сканировать список wi-fi сетей
		/// </summary>
		/// <returns></returns>
		internal static async Task<string> Wifi_List()
		{
			return await WebClient.Client.GetStringAsync("System/Network/interfaces/2/wireless/accessPointList");
		}

		/// <summary>
		/// Текущее отображение даты и времени на видео-потоке
		/// </summary>
		/// <returns></returns>
		internal static async Task<string> OsdDateTime()
		{
			return await WebClient.Client.GetStringAsync("System/Video/inputs/channels/1/overlays/dateTimeOverlay");
		}


		/// <summary>
		/// Текущее отображение имени устройства/канала на видео-потоке
		/// </summary>
		/// <returns></returns>
		internal static async Task<string> OsdChannelName()
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
		internal static StreamContent SerializeXmlData(object data)
		{
			MemoryStream ms = new();
			new XmlSerializer(data.GetType()).Serialize(ms, data);
			ms.Seek(0, SeekOrigin.Begin);
			StreamContent content = new(ms);
			return content;
		}
	
	}
}
