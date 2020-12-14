using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hikvision.RequestsData
{
	public static class DetectionData
	{
		/// <summary>
		/// Обязательный родительский класс для настройки действий при обнаружении движения
		/// </summary>
		public class EventTriggerNotificationList
		{
			[XmlElement("EventTriggerNotification")] public EventTriggerNotification EventTriggerNotification { get; set; }
		}
		
		/// <summary>
		/// Настройка действий при обнаружении движения
		/// </summary>
		public class EventTriggerNotification
		{
			[XmlElement("id")] public byte Id = 1; 
			[XmlElement("notificationMethod")] public string NotificationMethod = "email";
			[XmlElement("notificationRecurrence")] public string NotificationRecurrence = "recurring";
		}


		/// <summary>
		/// Настройка поведения детекции движения
		/// </summary>
		public class MotionDetection
		{
			[XmlElement("enabled")] public bool Enabled { get; set; }
			[XmlElement("enableHighlight")] public bool EnableHighlight { get; set; }
			[XmlElement("regionType")] public string RegionType = "grid";
			[XmlElement("Grid")] public Grid Grid { get; set; }
			[XmlElement("MotionDetectionLayout")] public MotionDetectionLayout MotionDetectionLayout { get; set; }
		}

		/// <summary>
		/// Постоянные значения, количество столбцов и строк в сетке
		/// </summary>
		public class Grid
		{
			[XmlElement("rowGranularity")] public byte RowGranularity = 18;
			[XmlElement("columnGranularity")] public byte ColumnGranularity = 22;
		}

		/// <summary>
		/// Настройка чувствительности и слоя детекции
		/// </summary>
		public class MotionDetectionLayout
		{
			[XmlElement("sensitivityLevel")] public byte SensitivityLevel { get; set; }
			[XmlElement("layout")] public Layout Layout { get; set; }
		}

		/// <summary>
		/// Слой сетки детекции
		/// </summary>
		public class Layout
		{
			[XmlElement("gridMap")] public string GridMap { get; set; }
		}
	}
}
