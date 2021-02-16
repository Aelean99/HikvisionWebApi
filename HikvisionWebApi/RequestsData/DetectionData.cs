using Newtonsoft.Json;

namespace Hikvision.RequestsData
{
	public class DetectionData
	{
		/// <summary>
		/// Настройка поведения детекции движения
		/// </summary>
		[JsonProperty( "MotionDetection" )]
		public MotionDetection motionDetection { get; set; }

		public class MotionDetection
		{
			[JsonProperty( "enabled" )] public bool Enabled { get; set; }
			[JsonProperty( "enableHighlight" )] public bool EnableHighlight { get; set; }
			[JsonProperty( "regionType" )] public string RegionType { get; set; }
			[JsonProperty( "Grid" )] public Grid Grid { get; set; }

			[JsonProperty( "MotionDetectionLayout" )]
			public MotionDetectionLayout MotionDetectionLayout { get; set; }
		}

		/// <summary>
		///      Постоянные значения, количество столбцов и строк в сетке
		/// </summary>
		public class Grid
		{
			public byte columnGranularity = 22;
			public byte rowGranularity = 18;
		}

		/// <summary>
		///      Настройка чувствительности и слоя детекции
		/// </summary>
		public class MotionDetectionLayout
		{
			public byte sensitivityLevel { get; set; }
			public Layout layout { get; set; }
		}

		/// <summary>
		///      Слой сетки детекции
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