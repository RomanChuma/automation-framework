using Newtonsoft.Json;

namespace AutomationFramework.Core.Utils.TestRail.Models.GetCaseFields
{
	public class Options
	{
		[JsonProperty("is_required")]
		public bool IsRequired { get; set; }

		[JsonProperty("default_value")]
		public string DefaultValue { get; set; }

		[JsonProperty("format")]
		public string Format { get; set; }

		[JsonProperty("rows")]
		public string Rows { get; set; }

		[JsonProperty("items")]
		public string Items { get; set; }
	}
}
