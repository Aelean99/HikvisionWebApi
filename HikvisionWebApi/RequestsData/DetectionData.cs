namespace Hikvision.RequestsData
{
	public class DetectionData
	{
		/// <summary>
		/// Обязательный родительский класс для настройки действий при обнаружении движения
		/// </summary>
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


		/// <summary>
		/// Настройка поведения детекции движения
		/// </summary>
		public class MotionDetection
		{
			public bool enabled { get; set; }
			public bool enableHighlight { get; set; }
			public string regionType { get; set; }
			public Grid Grid { get; set; }
			public MotionDetectionLayout MotionDetectionLayout { get; set; }
		}

		/// <summary>
		/// Постоянные значения, количество столбцов и строк в сетке
		/// </summary>
		public class Grid
		{
			public byte rowGranularity = 18;
			public byte columnGranularity = 22;
		}

		/// <summary>
		/// Настройка чувствительности и слоя детекции
		/// </summary>
		public class MotionDetectionLayout
		{
			public byte sensitivityLevel { get; set; }
			public Layout layout { get; set; }
		}

		/// <summary>
		/// Слой сетки детекции
		/// </summary>
		public class Layout
		{
			public string gridMap { get; set; }
		}

		public class GridFromDB
		{
			public uint id { get; set; }
			public string dbGridMap { get; set; }
		}

	}
}
