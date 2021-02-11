using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Hikvision.RequestsData
{
	public class TimeData
	{
		[JsonProperty("Time")] public Time time { get; set; }
		public class Time
		{
			public string timeMode { get; set; }
			public string timeZone { get; set; }
		}
	}
}
