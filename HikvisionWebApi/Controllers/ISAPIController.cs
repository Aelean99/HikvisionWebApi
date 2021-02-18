using Hikvision.Modules;
using Hikvision.RequestsData;

using Microsoft.AspNetCore.Mvc;

using NLog;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WebClient = Hikvision.Modules.WebClient;
// ReSharper disable All

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



	[ApiController, Route( "api/[controller]/" )]
	public class IsapiController : ControllerBase
	{
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

		private static int CheckClient( uint id, out string rtspIp )
		{
			_logger.Info( "[CheckClient] Method started" );
			var camListData = Task.Run( () => DBrequests.CameraGet( id ) ).Result; //получить данные о камере из базы 
			string cam_ip = string.Empty;

			foreach ( var item in camListData )
			{
				cam_ip = item.rtsp_ip; //извлекаем ip адрес для создания подключения с камерой
			}

			if ( cam_ip is null )
			{
				rtspIp = null;
				return 601;
			}

			var authStatus = 0;
			if ( WebClient.Client is null )  //Если клиент не соединён ни с одной из камер - то инициализировать соединие
			{
				_logger.Info( "[CheckClient] Client is an empty object. Object initialization" );
				authStatus = Task.Run( () => WebClient.InitClient( cam_ip ) ).Result;
			}
			else if ( WebClient.CamIp != cam_ip ) //Так же если у созданного соединения с камерой, старый ip отличается от нового, то создать подключение с новым ip адресом 
			{
				_logger.Info( $"[CheckClient] IP address of the current object [{WebClient.CamIp}] differs from the incoming one [{cam_ip}]. Initialization of a new object" );
				authStatus = Task.Run( () => WebClient.InitClient( cam_ip ) ).Result;
			}
			else if ( WebClient.CamIp == cam_ip && WebClient.CamStatusCode == 200 )
			{
				_logger.Info( "[CheckClient] Auth status returned 200" );
				authStatus = 200;
			}
			else if ( WebClient.CamIp == cam_ip && WebClient.CamStatusCode == 404 )
			{
				_logger.Info( "[CheckClient] Auth status returned 404" );
				authStatus = 404;
			}

			rtspIp = cam_ip;
			_logger.Info( "[CheckClient] Method has completed" );
			return authStatus;
		}

		[HttpPost, Route( "[action]" )]
		public async Task<string> ChangeDetectionMaskFromBody( [FromBody] DetectionData.GridFromDB data )
		{
			var authStatus = CheckClient( data.id, out string rtspIp );
			switch ( authStatus )
			{
				case 200:
					{
						_logger.Info( "[ChangeDetectionMaskFromBody] Auth status 200" );
						
						var response = await PutRequests.ChangeDetectionMaskFromBody( data );
						
						_logger.Info( "[ChangeDetectionMaskFromBody] Configuration is complete. \nMethod has completed" );
						return response;
					}
				case 401:
					{
						_logger.Info( "[ChangeDetectionMaskFromBody] Auth status 401. Unauthorized. \nMethod has completed" );
						return null;
					}
				case 404:
					{
						_logger.Info( "[ChangeDetectionMaskFromBody] Auth status 404. Device not supported. \nMethod has completed" );
						return null;
					}
				case 601:
					{
						_logger.Info( "[ChangeDetectionMaskFromBody] Status 601. Rtsp_ip is null. \nMethod has completed" );
						return null;
					}
				default:
					{
						_logger.Info( $"[ChangeDetectionMaskFromBody] Auth status {authStatus}. Default error. \nMethod has completed" );
						return null;
					}
			}
		}


		[HttpPost, Route( "[action]" )]
		public async Task<MassConfigData> GetAllConfigurations( [FromBody] CamId data )
		{
			_logger.Info( "[GetAllConfigurations] Method started" );
			var authStatus = CheckClient( data.Id, out string rtspIp );
			MassConfigData massConfig = new();
			switch ( authStatus )
			{
				case 200:
					{
						_logger.Info( "[GetAllConfigurations] Auth status 200. Start of configuration" );
						Console.WriteLine( $"Getting data from {rtspIp}" );
						massConfig.Id = data.Id;
						massConfig.NetworkData = await GetRequests.Ethernet();
						massConfig.DetectionData = await GetRequests.Detection();
						massConfig.EmailData = await GetRequests.Email();
						massConfig.TimeData = await GetRequests.Time();
						massConfig.NtpData = await GetRequests.Ntp();
						massConfig.OsdChannelNameData = await GetRequests.OsdChannelName();
						massConfig.OsdDateTimeData = await GetRequests.OsdDateTime();
						massConfig.StreamingData = await GetRequests.StreamingChannel();
						massConfig.EventTriggerData = await GetRequests.EventNotifications();

						Console.WriteLine( "Done" );
						_logger.Info( "[GetAllConfigurations] Configuration is complete. \nMethod has completed" );
						return massConfig;
					}
				case 401:
					{
						_logger.Info( "[GetAllConfigurations] Auth status 401. Unauthorized. \nMethod has completed" );
						return null;
					}
				case 404:
					{
						_logger.Info( "[GetAllConfigurations] Auth status 404. Device not supported. \nMethod has completed" );
						return null;
					}
				case 601:
					{
						_logger.Info( "[GetAllConfigurations] Status 601. Rtsp_ip is null. \nMethod has completed" );
						return null;
					}
				default:
					{
						_logger.Info( $"[GetAllConfigurations] Auth status {authStatus}. Default error. \nMethod has completed" );
						return null;
					}
			}
		}

		[HttpPost, Route( "[action]" )]
		public async Task<string> SetAllConfigurations( [FromBody] MassConfigData data )
		{

			var authStatus = CheckClient( data.Id, out string rtspIp );

			switch ( authStatus )
			{
				case 200:
					{
						Console.WriteLine( $"Setting {rtspIp}" );
						Console.WriteLine( await PutRequests.SetTimeFromBody( data.TimeData ) );
						Console.WriteLine( await PutRequests.SetNtpFromBody( data.NtpData ) );
						Console.WriteLine( await PutRequests.SetOsdChannelNameFromBody( data.OsdChannelNameData ) );
						Console.WriteLine( await PutRequests.SetOsdDateTimeFromBody( data.OsdDateTimeData ) );
						Console.WriteLine( await PutRequests.SetEmailFromBody( data.EmailData ) );
						Console.WriteLine( await PutRequests.SetDetectionFromBody( data.DetectionData ) );
						Console.WriteLine( await PutRequests.SetAlarmNotificationsFromBody( data.EventTriggerData ) );
						Console.WriteLine( await PutRequests.SetDnsFromBody( data.NetworkData ) );
						Console.WriteLine( await PutRequests.SetStreamConfigFromBody( data.StreamingData ) );
						Console.WriteLine( await PutRequests.ChangePassword() );
						Console.WriteLine( "Done" );

						return $"Status сode {authStatus}: OK";
					}
				case 401:
					{
						_logger.Info( "[SetAllConfigurations] Auth status 401. Unauthorized. \nMethod has completed" );
						return "Auth status 401. Unauthorized.";
					}
				case 404:
					{
						_logger.Info( "[SetAllConfigurations] Auth status 404. Device not supported. \nMethod has completed" );
						return "Auth status 404. Device not supported.";
					}
				case 601:
					{
						_logger.Info( "[SetAllConfigurations] Status 601. Rtsp_ip is null. \nMethod has completed" );
						return "Status 601. Rtsp_ip is null.";
					}
				default:
					{
						_logger.Info( $"[SetAllConfigurations] Auth status {authStatus}. Default error. \nMethod has completed" );
						return "Default error.";
					}
			}
		}
	}
}
