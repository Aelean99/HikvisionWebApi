using Newtonsoft.Json;

namespace Hikvision.RequestsData
{
	public class NotificationData
	{
		/// <summary>
		/// Обязательный родительский класс для настройки действий при обнаружении движения
		/// </summary>
		[JsonProperty( "EventTriggerNotificationList" )] public EventTriggerNotificationList eventTriggerNotificationList { get; set; }
		public class EventTriggerNotificationList
		{
			public EventTriggerNotification EventTriggerNotification { get; set; }
		}

		/// <summary>
		/// Настройка действий при обнаружении движения
		/// </summary>
		public class EventTriggerNotification
		{
			public string id { get; set; }
			public string notificationMethod { get; set; }
			public string notificationRecurrence { get; set; }
		}
	}
}
