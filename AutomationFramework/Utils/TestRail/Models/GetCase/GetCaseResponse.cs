using System.Collections.Generic;

using AutomationFramework.Core.Utils.TestRail.Models.GetTests;

using Newtonsoft.Json;

namespace AutomationFramework.Core.Utils.TestRail.Models.GetCase
{
	/// <summary>
	/// Data class for the response of 'get_case' API
	/// See http://docs.gurock.com/testrail-api2/reference-cases#get_case
	/// </summary>
	public class GetCaseResponse
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("section_id")]
		public int SectionId { get; set; }

		[JsonProperty("template_id")]
		public int TemplateId { get; set; }

		[JsonProperty("type_id")]
		public int TypeId { get; set; }

		[JsonProperty("priority_id")]
		public int PriorityId { get; set; }

		[JsonProperty("milestone_id")]
		public object MilestoneId { get; set; }

		[JsonProperty("refs")]
		public object Refs { get; set; }

		[JsonProperty("created_by")]
		public int CreatedBy { get; set; }

		[JsonProperty("created_on")]
		public int CreatedOn { get; set; }

		[JsonProperty("updated_by")]
		public int UpdatedBy { get; set; }

		[JsonProperty("updated_on")]
		public int UpdatedOn { get; set; }

		[JsonProperty("estimate")]
		public object Estimate { get; set; }

		[JsonProperty("estimate_forecast")]
		public string EstimateForecast { get; set; }

		[JsonProperty("suite_id")]
		public int SuiteId { get; set; }

		[JsonProperty("custom_tcstatus")]
		public int CustomTestCaseStatus { get; set; }

		[JsonProperty("custom_checlist")]
		public bool CustomChecklist { get; set; }

		[JsonProperty("custom_cqs_all_hard_to_reproduce")]
		public bool CustomCqsAllHardToReproduce { get; set; }

		[JsonProperty("custom_exectime")]
		public object CustomExectime { get; set; }

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
		public string CustomHistoryinformation { get; set; }
	}
}
