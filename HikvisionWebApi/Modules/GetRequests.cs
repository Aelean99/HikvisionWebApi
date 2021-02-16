using Hikvision.RequestsData;

using Newtonsoft.Json;

using NLog;

using System;
using System.Threading.Tasks;

namespace Hikvision.Modules
{
	public class GetRequests
	{
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();


		/// <summary>
		/// Запросить системную информацию с устойства
		/// </summary>
		public static async Task<CamResponses> DeviceInfo()
		{
			_logger.Info( "[DeviceInfo] Method started" );
			try
			{
				var response = await WebClient.Client.GetStringAsync( "System/deviceInfo" );
				var jsonResponse = Converters.XmlToJson( response );
				var deserializedObject = JsonConvert.DeserializeObject<CamResponses>( jsonResponse );
				_logger.Info( "[DeviceInfo] Method has complete" );
				return deserializedObject;
			}
			catch ( Exception e )
			{
				_logger.Error( $"[DeviceInfo] Error: {e.Message}" );
				return new CamResponses();
			}
		}


		/// <summary>
		/// Конфигурация сетевых адаптеров
		/// </summary>
		public static async Task<NetworkData> Ethernet()
		{
			_logger.Info( "[Ethernet] Method started" );
			try
			{
				var response = await WebClient.Client.GetStringAsync( "System/Network/interfaces/1/ipAddress" );
				var jsonResponse = Converters.XmlToJson( response );
				var deserializedObject = JsonConvert.DeserializeObject<NetworkData>( jsonResponse );
				_logger.Info( "[Ethernet] Method has complete" );
				return deserializedObject;
			}
			catch ( Exception e )
			{
				_logger.Error( $"[Ethernet] Error: {e.Message}" );
				return new NetworkData();
			}
		}


		/// <summary>
		/// Конфигурация настроек времени
		/// </summary>
		public static async Task<TimeData> Time()
		{
			_logger.Info( "[Time] Method started" );
			try
			{
				var response = await WebClient.Client.GetStringAsync( "System/time" );
				var jsonResponse = Converters.XmlToJson( response );
				var deserializedObject = JsonConvert.DeserializeObject<TimeData>( jsonResponse );
				_logger.Info( "[Time] Method has complete" );
				return deserializedObject;
			}
			catch ( Exception e )
			{
				_logger.Error( $"[Time] Error: {e.Message}" );
				return new TimeData();
			}
		}


		/// <summary>
		/// Конфигурация настроек времени
		/// </summary>
		public static async Task<NtpData> Ntp()
		{
			_logger.Info( "[Ntp] Method started" );
			try
			{
				var response = await WebClient.Client.GetStringAsync( "System/time/NtpServers/1" );
				var jsonResponse = Converters.XmlToJson( response );
				var deserializedObject = JsonConvert.DeserializeObject<NtpData>( jsonResponse );
				_logger.Info( "[Ntp] Method has complete" );
				return deserializedObject;
			}
			catch ( Exception e )
			{
				_logger.Error( $"[Ntp] Error: {e.Message}" );
				return new NtpData();
			}
		}


		/// <summary>
		/// Конфигурация SMTP протокола
		/// </summary>
		public static async Task<EmailData> Email()
		{
			_logger.Info( "[Email] Method started" );
			try
			{
				var response = await WebClient.Client.GetStringAsync( "System/Network/mailing/1" );
				var jsonResponse = Converters.XmlToJson( response );
				var deserializedObject = JsonConvert.DeserializeObject<EmailData>( jsonResponse );
				_logger.Info( "[Email] Method has complete" );
				return deserializedObject;
			}
			catch ( Exception e )
			{
				_logger.Error( $"[Email] Error: {e.Message}" );
				return new EmailData();
			}
		}


		/// <summary>
		/// Конфигурация детекции движения на устройстве
		/// </summary>
		public static async Task<DetectionData> Detection()
		{
			_logger.Info( "[Detection] Method started" );
			try
			{
				var response = await WebClient.Client.GetStringAsync( "System/Video/inputs/channels/1/motionDetection" );
				var jsonResponse = Converters.XmlToJson( response );
				var deserializedObject = JsonConvert.DeserializeObject<DetectionData>( jsonResponse );
				_logger.Info( "[Detection] Method has complete" );
				return deserializedObject;
			}
			catch ( Exception e )
			{
				_logger.Error( $"[Detection] Error: {e.Message}" );
				return new DetectionData();
			}
		}


		/// <summary>
		/// Список wi-fi сетей которые просканировало устройство
		/// </summary>
		public static async Task<string> WifiList()
		{
			try
			{
				return await WebClient.Client.GetStringAsync( "System/Network/interfaces/2/wireless/accessPointList" );
			}
			catch ( Exception e )
			{
				_logger.Error( $"[WifiList] Error: {e.Message}" );
				return e.Message;
			}
		}


		/// <summary>
		/// Текущее отображение даты и времени на видео-потоке
		/// </summary>
		public static async Task<OsdDatetimeData> OsdDateTime()
		{
			_logger.Info( "[OsdDatetimeData] Method started" );
			try
			{
				var response = await WebClient.Client.GetStringAsync( "System/Video/inputs/channels/1/overlays/dateTimeOverlay" );
				var jsonResponse = Converters.XmlToJson( response );
				var deserializedObject = JsonConvert.DeserializeObject<OsdDatetimeData>( jsonResponse );
				_logger.Info( "[OsdDatetimeData] Method has complete" );
				return deserializedObject;
			}
			catch ( Exception e )
			{
				_logger.Error( $"[OsdDatetimeData] Error: {e.Message}" );
				return new OsdDatetimeData();
			}
		}


		/// <summary>
		/// Текущее отображение имени устройства/канала на видео-потоке
		/// </summary>
		public static async Task<OsdChannelNameData> OsdChannelName()
		{
			_logger.Info( "[OsdChannelNameData] Method started" );
			try
			{
				var response = await WebClient.Client.GetStringAsync( "System/Video/inputs/channels/1/overlays/channelNameOverlay" );
				var jsonResponse = Converters.XmlToJson( response );
				var deserializedObject = JsonConvert.DeserializeObject<OsdChannelNameData>( jsonResponse );
				_logger.Info( "[OsdChannelNameData] Method has complete" );
				return deserializedObject;
			}
			catch ( Exception e )
			{
				_logger.Error( $"[OsdChannelNameData] Error: {e.Message}" );
				return new OsdChannelNameData();
			}
		}


		/// <summary>
		/// Текущая конфигурация с камеры
		/// </summary>
		public static async Task<StreamingData> StreamingChannel()
		{
			_logger.Info( "[StreamingChannel] Method started" );
			try
			{
				var response = await WebClient.Client.GetStringAsync( "Streaming/channels/101" );
				var jsonResponse = Converters.XmlToJson( response );
				var deserializedObject = JsonConvert.DeserializeObject<StreamingData>( jsonResponse );
				_logger.Info( "[StreamingChannel] Method has complete" );
				return deserializedObject;
			}
			catch ( Exception e )
			{
				_logger.Error( $"[StreamingChannel] Error: {e.Message}" );
				return new StreamingData();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static async Task<NotificationData> EventNotifications()
		{
			_logger.Info( "[EventNotifications] Method started" );
			try
			{
				var response = await WebClient.Client.GetStringAsync( "Event/triggers/VMD-1/notifications" );
				var jsonResponse = Converters.XmlToJson( response );
				var deserializedObject = JsonConvert.DeserializeObject<NotificationData>( jsonResponse );
				_logger.Info( "[EventNotifications] Method has complete" );
				return deserializedObject;
			}
			catch ( Exception e )
			{
				_logger.Error( $"[EventNotifications] Error: {e.Message}" );
				return new NotificationData();
			}
		}
	}
}
