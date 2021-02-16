using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Hikvision.RequestsData
{
	public class UserData
	{
		[JsonProperty( "User" )] public User user { get; set; }
		public class User
		{
			[JsonProperty("id")] public byte Id { get; set; }
			[JsonProperty( "userName" )] public string UserName { get; set; }
			[JsonProperty( "password" )] public string Password { get; set; }
		}
	}
}
