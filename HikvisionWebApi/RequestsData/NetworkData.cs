using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Hikvision.RequestsData
{
	/// <summary>
	/// Классы которые будут сериализованы в XML дату для отправки на камеру
	/// </summary>
	public class NetworkData
	{
		[JsonProperty( "IPAddress" )] public IPAddress ipAddress { get; set; }
		public class IPAddress
		{
			[JsonProperty( "ipVersion" )]public string IpVersion { get; set; }
			[JsonProperty( "addressingType" )]public string AddressingType { get; set; }
			[JsonProperty( "ipAddress" )] public string IpAddress { get; set; }
			[JsonProperty( "subnetMask" )] public string SubnetMask { get; set; }
			[JsonProperty( "DefaultGateway" )] public DefaultGateway DefaultGateway { get; set; }
			[JsonProperty( "PrimaryDNS" )] public PrimaryDNS PrimaryDNS { get; set; }
			[JsonProperty( "SecondaryDNS" )] public SecondaryDNS SecondaryDNS { get; set; }
			[JsonProperty( "Ipv6Mode" )] public Ipv6Mode Ipv6Mode { get; set; }
		}

		public class DefaultGateway
		{
			[JsonProperty( "ipAddress" )] public string IpAddress { get; set; }
		}
		public class PrimaryDNS
		{
			[JsonProperty( "ipAddress" )] public string IpAddress { get; set; }
		}
		public class SecondaryDNS
		{
			[JsonProperty( "ipAddress" )] public string IpAddress { get; set; }
		}

		public class Ipv6Mode
		{
			[JsonProperty( "ipV6AddressingType" )] public string IpV6AddressingType { get; set; }
			[JsonProperty( "ipv6AddressList" )] public Ipv6AddressList Ipv6AddressList { get; set; }
		}
		
		public class Ipv6AddressList
		{
			[JsonProperty( "v6Address" )] public V6Address v6Address { get; set; }
		}
		public class V6Address
		{
			[JsonProperty( "id" )] public byte Id { get; set; }
			[JsonProperty( "type" )] public string Type { get; set; }
			[JsonProperty( "address" )] public string Address { get; set; }
			[JsonProperty( "bitMask" )] public byte BitMask { get; set; }
		}


	}
}
