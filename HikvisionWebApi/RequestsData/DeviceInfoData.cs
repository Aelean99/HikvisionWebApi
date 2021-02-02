using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hikvision.RequestsData
{
	public class DeviceInfoData
	{
		public class DeviceInfo
		{
			public string serialNumber { get; set; }
			public string macAddress { get; set; }
		}
	}
}
