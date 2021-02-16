using Hikvision.Controllers;
using Hikvision.RequestsData;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using NLog;

using System;
using System.Text;
using System.Threading.Tasks;

namespace Hikvision.Modules
{
	public class PutRequests
	{
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// SMTP configuration
		/// </summary>
		public static async Task<string> SetEmailFromBody( [FromBody] EmailData data )
		{
			_logger.Info( "[SetEmailFromBody] Method started" );

			try
			{
				var deviceInfoData = await GetRequests.DeviceInfo();

				var serial = $"HK-{deviceInfoData.deviceInfo.serialNumber}@camera.ru";  //HK-serialNumber@camera.ru
				data.mailing.sender.emailAddress = serial;
				data.mailing.receiverList.receiver[0].emailAddress = serial;

				using var xmlData = Converters.ToStringContent( data, "mailing" );
				var response = await WebClient.Client.PutAsync( "System/Network/mailing/1", xmlData ).Result.Content.ReadAsStringAsync();
				var jsonResponse = Converters.XmlToJson( response );
				var responseData = JsonConvert.DeserializeObject<CamResponses>( jsonResponse );

				_logger.Info( "[SetEmailFromBody] Method has complete" );
				return $"{responseData.responseStatus.StatusString,-10} Request url {responseData.responseStatus.RequestUrl}";
			}
			catch ( Exception e )
			{
				_logger.Error( $"[SetEmailFromBody] Method failed. \nError message: {e.Message}" );
				return e.Message;
			}
		}

		/// <summary>
		/// Настройка NTP конфигурации
		/// </summary>
		public static async Task<string> SetNtpFromBody( [FromBody] NtpData data )
		{
			_logger.Info( "[SetNtpFromBody] Method started" );
			try
			{
				using var xmlData = Converters.ToStringContent( data, "NTPServer" );
				var response = await WebClient.Client.PutAsync( "System/time/NtpServers/1", xmlData ).Result.Content.ReadAsStringAsync();
				var jsonResponse = Converters.XmlToJson( response );
				var responseData = JsonConvert.DeserializeObject<CamResponses>( jsonResponse );

				_logger.Info( "[SetNtpFromBody] Method has complete" );
				return $"{responseData.responseStatus.StatusString,-10} Request url {responseData.responseStatus.RequestUrl}";
			}
			catch ( Exception e )
			{
				_logger.Error( $"[SetNtpFromBody] Method failed. \nError message: {e.Message}" );
				return e.Message;
			}
		}


		/// <summary>
		/// Настройка часового пояса
		/// </summary>
		public static async Task<string> SetTimeFromBody( [FromBody] TimeData data )
		{
			_logger.Info( "[SetTimeFromBody] Method started" );
			try
			{
				using var xmlData = Converters.ToStringContent( data, "Time" );
				var response = await WebClient.Client.PutAsync( "System/time", xmlData ).Result.Content.ReadAsStringAsync();
				var jsonResponse = Converters.XmlToJson( response );
				var responseData = JsonConvert.DeserializeObject<CamResponses>( jsonResponse );

				_logger.Info( "[SetTimeFromBody] Method has complete" );
				return $"{responseData.responseStatus.StatusString,-10} Request url {responseData.responseStatus.RequestUrl}";
			}
			catch ( Exception e )
			{
				_logger.Error( $"[SetTimeFromBody] Method failed. \nError message: {e.Message}" );
				return e.Message;
			}
		}


		/// <summary>
		/// Настройка видео-аудио конфигурации
		/// </summary>
		public static async Task<string> SetStreamConfigFromBody( [FromBody] StreamingData data )
		{
			_logger.Info( "[SetStreamConfigFromBody] Method started" );
			try
			{
				using var xmlData = Converters.ToStringContent( data, "StreamingChannel" );
				var response = await WebClient.Client.PutAsync( "Streaming/channels/101", xmlData ).Result.Content.ReadAsStringAsync();
				var jsonResponse = Converters.XmlToJson( response );
				var responseData = JsonConvert.DeserializeObject<CamResponses>( jsonResponse );

				_logger.Info( "[SetTimeFromBody] Method has complete" );
				return $"{responseData.responseStatus.StatusString,-10} Request url {responseData.responseStatus.RequestUrl}";
			}
			catch ( Exception e )
			{
				_logger.Error( $"[SetStreamConfigFromBody] Method failed. \nError message: {e.Message}" );
				return e.Message;
			}
		}


		/// <summary>
		/// Change DNS on device
		/// </summary>
		public static async Task<string> SetDnsFromBody( [FromBody] NetworkData data )
		{
			_logger.Info( "[SetDnsFromBody] Method started" );
			// запрашиваем конфигурацию сети с камеры
			// без этих данных камера не примет XML
			var networkData = await GetRequests.Ethernet();

			//извлекаем текущие значения конфигурации сети
			var addressingType = networkData.ipAddress.AddressingType;

			if (addressingType != "static") return $"Addressing type is {addressingType}. Can`t set DNS";
			var ipAddress = networkData.ipAddress.IpAddress;
			var subnetMask = networkData.ipAddress.SubnetMask;
			var defaultGateway = networkData.ipAddress.DefaultGateway.IpAddress;

			//дополняем пришедший json полученными значениями с камер
			data.ipAddress.IpAddress = ipAddress;
			data.ipAddress.SubnetMask = subnetMask;
			data.ipAddress.DefaultGateway.IpAddress = defaultGateway;

			try
			{
				using var xmlData = Converters.ToStringContent( data, "IPAddress" );
				var response = await WebClient.Client.PutAsync( "System/Network/interfaces/1/ipAddress", xmlData ).Result.Content.ReadAsStringAsync();
				var jsonResponse = Converters.XmlToJson( response );
				var responseData = JsonConvert.DeserializeObject<CamResponses>( jsonResponse );

				_logger.Info( "[SetDnsFromBody] Method has complete" );
				return $"{responseData.responseStatus.StatusString,-10} Request url {responseData.responseStatus.RequestUrl}";
			}
			catch ( Exception e )
			{
				_logger.Error( $"[SetDnsFromBody] Method failed. \nError message: {e.Message}" );
				return e.Message;
			}
		}


		/// <summary>
		/// Настройка отображения даты и времени на видео-потоке
		/// </summary>
		public static async Task<string> SetOsdChannelNameFromBody( [FromBody] OsdChannelNameData data )
		{
			_logger.Info( "[SetOsdChannelNameFromBody] Method started" );
			try
			{
				using var xmlData = Converters.ToStringContent( data, "channelNameOverlay" );
				var response = await WebClient.Client.PutAsync( "System/Video/inputs/channels/1/overlays/channelNameOverlay", xmlData ).Result.Content.ReadAsStringAsync();
				var jsonResponse = Converters.XmlToJson( response );
				var responseData = JsonConvert.DeserializeObject<CamResponses>( jsonResponse );

				_logger.Info( "[SetOsdChannelNameFromBody] Method has complete" );
				return $"{responseData.responseStatus.StatusString,-10} Request url {responseData.responseStatus.RequestUrl}";
			}
			catch ( Exception e )
			{
				_logger.Error( $"[SetOsdChannelNameFromBody] Method failed. \nError message: {e.Message}" );
				return e.Message;
			}
		}


		/// <summary>
		/// Настройка отображения времени на канале
		/// Отключение отображения
		/// </summary>
		public static async Task<string> SetOsdDateTimeFromBody( [FromBody] OsdDatetimeData data )
		{
			_logger.Info( "[SetOsdDateTimeFromBody] Method started" );
			try
			{
				using var xmlData = Converters.ToStringContent( data, "dateTimeOverlay " );
				var response = await WebClient.Client.PutAsync( "System/Video/inputs/channels/1/overlays/dateTimeOverlay", xmlData ).Result.Content.ReadAsStringAsync();
				var jsonResponse = Converters.XmlToJson( response );
				var responseData = JsonConvert.DeserializeObject<CamResponses>( jsonResponse );

				_logger.Info( "[SetOsdDateTimeFromBody] Method has complete" );
				return $"{responseData.responseStatus.StatusString,-10} Request url {responseData.responseStatus.RequestUrl}";
			}
			catch ( Exception e )
			{
				_logger.Error( $"[SetOsdDateTimeFromBody] Method failed. \nError message: {e.Message}" );
				return e.Message;
			}
		}


		/// <summary>
		/// Настройка способа отправки детекции
		/// </summary>
		public static async Task<string> SetAlarmNotificationsFromBody( [FromBody] NotificationData data )
		{
			_logger.Info( "[SetAlarmNotificationsFromBody] Method started" );
			try
			{
				using var xmlData = Converters.ToStringContent( data, "EventTriggerNotificationList" );
				var response = await WebClient.Client.PutAsync( "Event/triggers/VMD-1/notifications", xmlData ).Result.Content.ReadAsStringAsync();
				var jsonResponse = Converters.XmlToJson( response );
				var responseData = JsonConvert.DeserializeObject<CamResponses>( jsonResponse );

				_logger.Info( "[SetAlarmNotificationsFromBody] Method has complete" );
				return $"{responseData.responseStatus.StatusString,-10} Request url {responseData.responseStatus.RequestUrl}";
			}
			catch ( Exception e )
			{
				_logger.Error( $"[SetAlarmNotificationsFromBody] Method failed. \nError message: {e.Message}" );
				return e.Message;
			}
		}


		/// <summary>
		/// Включение детекции движения с предустановленной заполенной маской детекции
		/// </summary>
		public static async Task<string> SetDetectionFromBody( [FromBody] DetectionData data )
		{
			_logger.Info( "[SetDetectionFromBody] Method started" );
			try
			{
				using var xmlData = Converters.ToStringContent( data, "MotionDetection" );
				var response = await WebClient.Client.PutAsync( "System/Video/inputs/channels/1/motionDetection", xmlData ).Result.Content.ReadAsStringAsync();
				var jsonResponse = Converters.XmlToJson( response );
				var responseData = JsonConvert.DeserializeObject<CamResponses>( jsonResponse );

				_logger.Info( "[SetDetectionFromBody] Method has complete" );
				return $"{responseData.responseStatus.StatusString,-10} Request url {responseData.responseStatus.RequestUrl}";
			}
			catch ( Exception e )
			{
				_logger.Error( $"[SetDetectionFromBody] Method failed. \nError message: {e.Message}" );
				return e.Message;
			}
		}


		public static async Task<string> ChangePassword()
		{
			_logger.Info( "[ChangePassword] Method started" );
			try
			{
				var userData = new UserData.User
				{
					Id = 1,
					UserName = "admin",
					Password = WebClient.Password
				};
				using var xmlData = Converters.ToStringContent( userData, "User" );
				var response = await WebClient.Client.PutAsync( "Security/users/1", xmlData ).Result.Content.ReadAsStringAsync();
				var jsonResponse = Converters.XmlToJson(response);
				var responseData = JsonConvert.DeserializeObject<CamResponses>(jsonResponse);

				_logger.Info( "[ChangePassword] Method has complete" );
				return $"{responseData.responseStatus.StatusString,-10} Request url {responseData.responseStatus.RequestUrl}";
			}
			catch (Exception e)
			{
				_logger.Error( $"[ChangePassword] Method failed. \nError message: {e.Message}" );
				return e.Message;
			}
		}


		/// <summary>
		/// Метод повторной смены маски детекции
		/// </summary>
		public static async Task<string> ChangeDetectionMaskFromBody( [FromBody] DetectionData.GridFromDB data )
		{
			_logger.Info( "[ChangeDetectionMaskFromBody] Method started" );

			byte[] hexValues = { 8, 4, 2, 1 }; // Значения для конвертирования входящей маски в понятный для камеры вид 
			StringBuilder gridMap = new();  // объявление итоговой маски для камеры

			try
			{
				var splittedArray = data.dbGridMap.Split( 22 ); // Маска поступает из 396 значений, разделить их на многомерный массив по 22 значения
				var sum = 0; // сумма значений в массиве на основе hexValues
				foreach ( var value in splittedArray ) // Выбрать получившиеся массивы разделённые по 22 значения
				{
					foreach ( var value2 in value.Split( 4 ) ) // раделить полученные ранее массивы по 22 элемента - ещё по 4
					{
						for ( var i = 0; i < value2.Length; i++ )
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

				using var xmlData = Converters.ToStringContent( objData, "MotionDetectionGridLayout" );
				var response = await WebClient.Client.PutAsync( "System/Video/inputs/channels/1/motionDetection/layout/gridLayout", xmlData ).Result.Content.ReadAsStringAsync();
				var jsonResponse = Converters.XmlToJson( response );
				var responseData = JsonConvert.DeserializeObject<CamResponses>( jsonResponse );

				_logger.Info( "[ChangeDetectionMaskFromBody] Method has complete" );
				return $"{responseData.responseStatus.StatusString,-5} Request url {responseData.responseStatus.RequestUrl}";
			}
			catch ( Exception e )
			{
				_logger.Error( $"[ChangeDetectionMaskFromBody] Method failed. \nError message: {e.Message}" );
				return e.Message;
			}
		}
	}
}
