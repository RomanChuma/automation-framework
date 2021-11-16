using Newtonsoft.Json;

namespace AutomationFramework.Core.Utils.TestRail.Models.GetCaseTypes
{
	/// <summary>
	/// Data class for the response of 'get_case_types' API call
	/// See http://docs.gurock.com/testrail-api2/reference-cases-types
	/// </summary>
	public class GetCaseTypesResponse
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("is_default")]
		public bool IsDefault { get; set; }
	}
}
