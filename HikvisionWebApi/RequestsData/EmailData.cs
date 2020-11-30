using System;
using System.Xml.Serialization;

namespace Hikvision.RequestsData
{
	public static class EmailData
	{
		[Serializable]
		public class mailing
		{
			[XmlElement("id")] public byte Id = 1;
			[XmlElement("sender")] public Sender Sender { get; set; }
			[XmlElement("receiverList")] public ReceiverList ReceiverList { get; set; }
			[XmlElement("attachment")] public Attachment Attachment { get; set; }
		}

		public class Sender
		{
			[XmlElement("emailAddress")] public string EmailAddress = "hikvisioncam@cam.ru";
			[XmlElement("name")] public string Name = "camera";
			[XmlElement("smtp")] public Smtp Smtp { get; set; }
		}

		public class Smtp
		{
			[XmlElement("enableAuthorization")] public bool EnableAuthorization = false;
			[XmlElement("enableSSL")] public bool EnableSsl = false;
			[XmlElement("addressingFormatType")] public string AddressingFormatType = "hostname";
			[XmlElement("hostName")] public string HostName { get; set; }
			[XmlElement("portNo")] public int PortNo { get; set; }
		}

		public class ReceiverList
		{
			[XmlElement("receiver")] public Receiver Receiver { get; set; }
		}

		public class Receiver
		{
			[XmlElement("id")] public byte Id = 1;
			[XmlElement("name")] public string Name = "camera";
			[XmlElement("emailAddress")] public string EmailAddress = "hikvisioncam@cam.ru";
		}

		public class Attachment
		{
			[XmlElement("snapshot")] public Snapshot Snapshot { get; set; }
		}

		public class Snapshot
		{
			[XmlElement("enabled")] public bool Enabled = false;
			[XmlElement("interval")] public int Interval = 2;
		}
	}
}
