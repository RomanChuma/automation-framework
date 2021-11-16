using Newtonsoft.Json;

namespace AutomationFramework.Core.Utils.TestRail.Models.GetTests
{
	public class CustomStepsSeparated
	{
		[JsonProperty("content")]
		public string Content { get; set; }

		[JsonProperty("expected")]
		public string Expected { get; set; }
	}
}
