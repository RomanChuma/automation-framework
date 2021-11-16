using System.Collections.Generic;

using Newtonsoft.Json;

namespace AutomationFramework.Core.Utils.TestRail.Models.GetTests
{
	/// <summary>
	/// Data class representing the response of 'get_tests' API call of TestRail API
	/// See http://docs.gurock.com/testrail-api2/reference-tests#get_tests
	/// </summary>
	public class GetTestsResponse
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("case_id")]
		public int CaseId { get; set; }

		[JsonProperty("status_id")]
		public int StatusId { get; set; }

		[JsonProperty("assignedto_id")]
		public object AssignedtoId { get; set; }

		[JsonProperty("run_id")]
		public int RunId { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("template_id")]
		public int TemplateId { get; set; }

		[JsonProperty("type_id")]
		public int TypeId { get; set; }

		[JsonProperty("priority_id")]
		public int PriorityId { get; set; }

		[JsonProperty("estimate")]
		public object Estimate { get; set; }

		[JsonProperty("estimate_forecast")]
		public string EstimateForecast { get; set; }

		[JsonProperty("refs")]
		public object Refs { get; set; }

		[JsonProperty("milestone_id")]
		public object MilestoneId { get; set; }

		[JsonProperty("custom_tcstatus")]
		public int CustomTcstatus { get; set; }

		[JsonProperty("custom_checlist")]
		public bool CustomChecklist { get; set; }

		[JsonProperty("custom_cqs_all_hard_to_reproduce")]
		public bool CustomCqsAllHardToReproduce { get; set; }

		[JsonProperty("custom_exectime")]
		public object CustomExecutionTime { get; set; }

		[JsonProperty("custom_auto_priority")]
		public object CustomAutoPriority { get; set; }

		[JsonProperty("custom_autoplatform")]
		public int? CustomAutoplatform { get; set; }

		[JsonProperty("custom_confluence_links")]
		public object CustomConfluenceLinks { get; set; }

		[JsonProperty("custom_preconds")]
		public string CustomPreconditions { get; set; }

		[JsonProperty("custom_steps_separated")]
		public List<CustomStepsSeparated> CustomStepsSeparated { get; set; }

		[JsonProperty("custom_references")]
		public object CustomReferences { get; set; }

		[JsonProperty("custom_comments")]
		public string CustomComments { get; set; }

		[JsonProperty("custom_histroryinformation")]
		public string CustomHistoryInformation { get; set; }
	}
}
