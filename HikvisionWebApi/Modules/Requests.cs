using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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
		public static async Task<string> Email()
		{
			var test = new EmailData.mailing
			{
				Sender = new EmailData.Sender() { Smtp = new EmailData.Smtp() },
				Attachment = new EmailData.Attachment { Snapshot = new EmailData.Snapshot() },
				ReceiverList = new EmailData.ReceiverList() { Receiver = new EmailData.Receiver() }
			};

			
			MemoryStream ms = new ();
			new XmlSerializer(test.GetType()).Serialize(ms, test);
			ms.Seek(0, SeekOrigin.Begin);
			StreamContent content = new (ms);
			return await WebClient.Client.PutAsync("System/Network/mailing/1", content).Result.Content.ReadAsStringAsync();
		}

	}
}
