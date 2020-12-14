using System.Xml.Serialization;

namespace Hikvision.RequestsData
{
	/// <summary>
	/// Классы которые будут сериализованы в XML дату для отправки на камеру
	/// </summary>
	public static class NetworkData
	{
		public class IPAddress
		{
			[XmlElement("ipVersion")] public string IpVersion = "dual";
			[XmlElement("addressingType")] public string AddressingType = "static";
			[XmlElement("ipAddress")] public string IpAddress { get; set; }
			[XmlElement("subnetMask")] public string SubnetMask { get; set; }
			[XmlElement("DefaultGateway")] public DefaultGateway DefaultGateway { get; set; }
			[XmlElement("PrimaryDNS")] public PrimaryDNS PrimaryDns { get; set; }
			[XmlElement("SecondaryDNS")] public SecondaryDNS SecondaryDns { get; set; }
			[XmlElement("Ipv6Mode")] public Ipv6Mode Ipv6Mode { get; set; }
		}

		public class DefaultGateway
		{
			[XmlElement("ipAddress")] public string IpAddress { get; set; }
		}
		public class PrimaryDNS
		{
			[XmlElement("ipAddress")] public string IpAddress { get; set; }
		}
		public class SecondaryDNS
		{
			[XmlElement("ipAddress")] public string IpAddress { get; set; }
		}

		public class Ipv6Mode
		{
			[XmlElement("ipV6AddressingType")] public string IpV6AddressingType = "ra";
			[XmlElement("ipv6AddressList")] public Ipv6AddressList Ipv6AddressList { get; set; }
		}
		
		public class Ipv6AddressList
		{
			[XmlElement("v6Address")] public V6Address V6Address { get; set; }
		}
		public class V6Address
		{
			[XmlElement("id")] public byte Id = 1;
			[XmlElement("type")] public string Type = "manual";
			[XmlElement("address")] public string Address = "::";
			[XmlElement("bitMask")] public byte BitMask = 0;

		}


	}
}
