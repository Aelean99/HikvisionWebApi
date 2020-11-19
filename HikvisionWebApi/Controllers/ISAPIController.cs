using HikvisionWebApi.Modules;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HikvisionWebApi
{

	  [ApiController, Route("api/[controller]/")]
	  public class ISAPIController : ControllerBase
	  {
			/// <summary>
			/// Проверка пароля учётной записи администратора
			/// </summary>
			/// <param name="ip">ip address</param>
			/// <returns>
			///	  200 - если введённый пароль совпадает с тем что на камере
			///	  401 - пароль не прошёл проверку
			/// </returns>
			[HttpGet, Route("[action]")]
			public async Task<string> UserCheck(string ip)
			{
				  return await Requests.UserCheck(ip);
			}

			[HttpGet, Route("[action]")]
			public async Task<string> DeviceInfo()
			{
				  return await Requests.DeviceInfo();
			}
	  }
}
