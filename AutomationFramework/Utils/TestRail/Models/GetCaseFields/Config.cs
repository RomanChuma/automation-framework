using Newtonsoft.Json;

namespace AutomationFramework.Core.Utils.TestRail.Models.GetCaseFields
{
	public class Config
	{
		[JsonProperty("context")]
		public Context Context { get; set; }

		[JsonProperty("options")]
		public Options Options { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }
	}
}
