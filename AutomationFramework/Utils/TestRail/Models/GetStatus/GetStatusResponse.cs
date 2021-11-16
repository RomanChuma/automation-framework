using Newtonsoft.Json;

namespace AutomationFramework.Core.Utils.TestRail.Models.GetStatus
{
	/// <summary>
	/// Data class for 'get_statuses' API call
	/// http://docs.gurock.com/testrail-api2/reference-statuses
	/// </summary>
	public class GetStatusResponse
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("label")]
		public string Label { get; set; }

		[JsonProperty("color_dark")]
		public int ColorDark { get; set; }

		[JsonProperty("color_medium")]
		public int ColorMedium { get; set; }

		[JsonProperty("color_bright")]
		public int ColorBright { get; set; }

		[JsonProperty("is_system")]
		public bool IsSystem { get; set; }

		[JsonProperty("is_untested")]
		public bool IsUntested { get; set; }

		[JsonProperty("is_final")]
		public bool IsFinal { get; set; }
	}
}
