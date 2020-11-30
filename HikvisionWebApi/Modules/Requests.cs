using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Hikvision.RequestsData;

namespace Hikvision.Modules
{
	internal static class Requests
	{
		public static async Task<string> DeviceInfo()
		{
			return await WebClient.Client.GetStringAsync("System/deviceInfo");
		}

		public static async Task<string> Time()
		{
			return await WebClient.Client.GetStringAsync("System/time");
		}

		public static async Task<string> Ethernet()
		{
			return await WebClient.Client.GetStringAsync("System/Network/interfaces/1/ipAddress");
		}                     

		public static async Task<string> Email()
		{
			return await WebClient.Client.GetStringAsync("System/Network/mailing");
		}

		public static async Task<string> Detection()
		{
			return await WebClient.Client.GetStringAsync("System/Video/inputs/channels/1/motionDetection");
		}

		public static async Task<string> Wifi_List()
		{
			return await WebClient.Client.GetStringAsync("System/Network/interfaces/2/wireless/accessPointList");
		}
	}

	internal static class Put
	{
		private static StreamContent SerializeXmlData(object data)
		{
			MemoryStream ms = new();
			new XmlSerializer(data.GetType()).Serialize(ms, data);
			ms.Seek(0, SeekOrigin.Begin);
			StreamContent content = new(ms);
			return content;
		}
		public static async Task<string> Email(string smtpServer, int port)
		{
			if (port is 0)
				port = new Random().Next(15005, 15007);

			var data = new EmailData.mailing
			{
				Sender = new EmailData.Sender { Smtp = new EmailData.Smtp { HostName = smtpServer, PortNo = port} },
				Attachment = new EmailData.Attachment { Snapshot = new EmailData.Snapshot() },
				ReceiverList = new EmailData.ReceiverList { Receiver = new EmailData.Receiver() }
			};
			using var content = SerializeXmlData(data);
			return await WebClient.Client.PutAsync("System/Network/mailing/1", content).Result.Content.ReadAsStringAsync();
		}

		public static async Task<string> Ntp(string ip, string addressFormatType)
		{
			var data = new TimeData.NTPServer()
			{
				IpAddress = ip, 
				AddressingFormatType = addressFormatType
			};
			using var content = SerializeXmlData(data);
			return await WebClient.Client.PutAsync("System/time/NtpServers", content).Result.Content.ReadAsStringAsync();
		}

		public static async Task<string> Time(string timezone)
		{
			var data = new TimeData.Time { TimeZone = timezone };
			using var content = SerializeXmlData(data);
			return await WebClient.Client.PutAsync("System/time", content).Result.Content.ReadAsStringAsync();
		}

		public static async Task<string> StreamingChannel(
			int videoResolutionWidth, 
			int videoResolutionHeight, 
			int maxBitrate, 
			string videoCodec, 
			bool audioEnabled, 
			string audioCompressType)
		{
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
			return await WebClient.Client.PutAsync("Streaming/channels/101", content).Result.Content.ReadAsStringAsync();
		}

	}
}
