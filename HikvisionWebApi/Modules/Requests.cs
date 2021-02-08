using System;
using System.Threading.Tasks;
using System.Xml;
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
		public static JObject ToJObject(object data)
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

		public static string XmlToJson( string data )
		{
			try
			{
				XmlDocument doc = new();
				doc.LoadXml( data ?? string.Empty );
				var jsonContent = JsonConvert.SerializeXmlNode( doc );
				return jsonContent;
			}
			catch ( Exception e )
			{
				return e.Message;
			}
		}

		/// <summary>
		/// Получить системную конфигурацию устройства(MAC адрес, serial номер и т.п)
		/// </summary>
		/// <returns></returns>
		internal static async Task<string> DeviceInfo()
		{

			try
			{
				return await WebClient.Client.GetStringAsync( "System/deviceInfo" );
			}
			catch ( Exception e ) { return e.Message; }
		}

		/// <summary>
		/// Получить конфигурацию настроек времени
		/// </summary>
		/// <returns></returns>
		internal static async Task<string> Time()
		{
			try
			{
				return await WebClient.Client.GetStringAsync( "System/time" );
			}
			catch ( Exception e ) { return e.Message; }
		}

		/// <summary>
		/// Получить кофнигурацию сетевых интерфейсов
		/// </summary>
		/// <returns></returns>
		internal static async Task<string> Ethernet()
		{
			try
			{
				return await WebClient.Client.GetStringAsync( "System/Network/interfaces/1/ipAddress" );
			}
			catch ( Exception e ) { return e.Message; }
		}                     

		/// <summary>
		/// Получить конфигурацию SMTP
		/// </summary>
		/// <returns></returns>
		internal static async Task<string> Email()
		{

			try
			{
				return await WebClient.Client.GetStringAsync( "System/Network/mailing" );
			}
			catch ( Exception e ) { return e.Message; }
		}

		/// <summary>
		/// Получить конфигурацию детекции движения
		/// </summary>
		/// <returns></returns>
		internal static async Task<string> Detection()
		{
			try
			{
				return await WebClient.Client.GetStringAsync( "System/Video/inputs/channels/1/motionDetection" );
			}
			catch ( Exception e ) { return e.Message; }
		}

		/// <summary>
		/// Сканировать список wi-fi сетей
		/// </summary>
		/// <returns></returns>
		internal static async Task<string> Wifi_List()
		{
			try
			{
				return await WebClient.Client.GetStringAsync( "System/Network/interfaces/2/wireless/accessPointList" );
			}
			catch ( Exception e ) { return e.Message; }
		}

		/// <summary>
		/// Текущее отображение даты и времени на видео-потоке
		/// </summary>
		/// <returns></returns>
		internal static async Task<string> OsdDateTime()
		{
			try
			{
				return await WebClient.Client.GetStringAsync( "System/Video/inputs/channels/1/overlays/dateTimeOverlay" );
			}
			catch ( Exception e ) { return e.Message; }
		}


		/// <summary>
		/// Текущее отображение имени устройства/канала на видео-потоке
		/// </summary>
		/// <returns></returns>
		internal static async Task<string> OsdChannelName()
		{
			try
			{
				return await WebClient.Client.GetStringAsync( "System/Video/inputs/channels/1/overlays/channelNameOverlay" );
			}
			catch ( Exception e ) { return e.Message; }
		}
	}
}
