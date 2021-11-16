using Newtonsoft.Json;

namespace AutomationFramework.Core.Utils.TestRail.Models.AddResult
{
	/// <summary>
	/// Data class for response of 'add_result' API
	/// </summary>
	public class AddResultResponse
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("test_id")]
		public int TestId { get; set; }

		[JsonProperty("status_id")]
		public int StatusId { get; set; }

		[JsonProperty("created_by")]
		public int CreatedBy { get; set; }

		[JsonProperty("created_on")]
		public int CreatedOn { get; set; }

		[JsonProperty("assignedto_id")]
		public object AssignedtoId { get; set; }

		[JsonProperty("comment")]
		public string Comment { get; set; }

		[JsonProperty("version")]
		public object Version { get; set; }

		[JsonProperty("elapsed")]
		public object Elapsed { get; set; }

		[JsonProperty("defects")]
		public object Defects { get; set; }

		[JsonProperty("custom_stepresults")]
		public object CustomStepResults { get; set; }
	}
}
