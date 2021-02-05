using System.Xml.Serialization;

namespace Hikvision.RequestsData
{
	/// <summary>
	/// Классы которые будут сериализованы в XML дату для отправки на камеру
	/// </summary>
	public class NetworkData
	{
		public class RootData
		{
			public string ipVersion { get; set; }
			public string addressingType { get; set; }
			public string ipAddress { get; set; }
			public string subnetMask { get; set; }
			public DefaultGateway DefaultGateway { get; set; }
			public PrimaryDNS PrimaryDNS { get; set; }
			public SecondaryDNS SecondaryDNS { get; set; }
			public Ipv6Mode Ipv6Mode { get; set; }
		}

		public class DefaultGateway
		{
			public string ipAddress { get; set; }
		}
		public class PrimaryDNS
		{
			public string ipAddress { get; set; }
		}
		public class SecondaryDNS
		{
			public string ipAddress { get; set; }
		}

		public class Ipv6Mode
		{
			public string ipV6AddressingType { get; set; }
			public Ipv6AddressList ipv6AddressList { get; set; }
		}
		
		public class Ipv6AddressList
		{
			public V6Address v6Address { get; set; }
		}
		public class V6Address
		{
			public byte id { get; set; }
			public string type { get; set; }
			public string address { get; set; }
			public byte bitMask { get; set; }
		}


	}
}
