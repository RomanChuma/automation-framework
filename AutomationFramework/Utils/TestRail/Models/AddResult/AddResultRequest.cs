using Newtonsoft.Json;

namespace AutomationFramework.Core.Utils.TestRail.Models.AddResult
{
	/// <summary>
	/// Data class for 'add_result' API
	/// http://docs.gurock.com/testrail-api2/reference-results#add_result
	/// </summary>
	public class AddResultRequest
	{
		/// <summary>
		/// Gets or sets the ID of the test status
		/// </summary>
		[JsonProperty("status_id")]
		public int StatusId { get; set; }

		/// <summary>
		/// Gets or sets the comment / description for the test result
		/// </summary>
		[JsonProperty("comment")]
		public string Comment { get; set; }

		/// <summary>
		/// Gets or sets te time it took to execute the test, e.g. "30s" or "1m 45s"
		/// </summary>
		[JsonProperty("elapsed")]
		public string Elapsed { get; set; }

		/// <summary>
		/// Gets or sets a comma-separated list of defects to link to the test result
		/// </summary>
		[JsonProperty("defects")]
		public string Defects { get; set; }

		/// <summary>
		/// Gets or sets the version or build you tested against
		/// </summary>
		[JsonProperty("version")]
		public string Version { get; set; }
	}
}
