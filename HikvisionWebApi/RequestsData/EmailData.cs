using System.Collections.Generic;
using Newtonsoft.Json;

namespace Hikvision.RequestsData
{
	public class EmailData
	{
		[JsonProperty( "mailing" )] public Mailing mailing { get; set; }
		public class Mailing
		{
			public byte id { get; set; }
			public Sender sender { get; set; }
			public List<ReceiverList> receiverList { get; set; }
			public Attachment attachment { get; set; }
		}

		public class Sender
		{
			public string name { get; set; }
			public string emailAddress { get; set; }
			public Smtp smtp { get; set; }
		}

		public class Smtp
		{
			public bool enableAuthorization { get; set; }
			public bool enableSSL { get; set; }
			public string addressingFormatType { get; set; }
			public string hostName { get; set; }
			public int portNo { get; set; }
			public string accountName { get; set; }
			public string password { get; set; }
		}

		public class ReceiverList
		{
			public Receiver receiver { get; set; }
		}

		public class Receiver
		{
			public byte id { get; set; }
			public string name { get; set; }
			public string emailAddress { get; set; }
		}

		public class Attachment
		{
			public Snapshot snapshot { get; set; }
		}

		public class Snapshot
		{
			public bool enabled { get; set; }
			public int interval { get; set; }
		}
	}
}
