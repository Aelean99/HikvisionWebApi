using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Net.Http;
using System.Xml;

namespace Hikvision.Modules
{
	internal class Converters
	{
		/// <summary>
		/// Метод преобразует XML ответ устройства в jObject для взаимодействия с возможностями json
		/// </summary>
		/// <param name="data">XML объект из которого в дальнейшем будут извлечены значения</param>
		/// <returns>десериализованный jobject объект</returns>
		public static JObject ToJObject( object data )
		{
			try
			{
				XmlDocument doc = new();
				doc.LoadXml( data.ToString() ?? string.Empty );
				var jsonContent = JsonConvert.SerializeXmlNode( doc );
				return (JObject) JsonConvert.DeserializeObject( jsonContent );
			}
			catch ( Exception e )
			{
				return new JObject();
			}
		}

		public static string XmlToJson( string data )
		{
			try
			{
				XmlDocument doc = new();
				doc.LoadXml( data ?? string.Empty );
				var jsonContent = JsonConvert.SerializeXmlNode( doc );
				return jsonContent;
			}
			catch ( Exception e )
			{
				return e.Message;
			}
		}

		public static StringContent ToStringContent( object data, string rootName )
		{
			var jsonData = JsonConvert.SerializeObject( data ); //data to jsonObject
			var xmlString = JsonConvert.DeserializeXNode( jsonData, rootName )?.ToString();  // jsonObject to xmlString
			StringContent xmlData = new( xmlString ?? string.Empty );    // xmlString to StringContent(http content put/post methods)
			return xmlData;
		}
	}
}
