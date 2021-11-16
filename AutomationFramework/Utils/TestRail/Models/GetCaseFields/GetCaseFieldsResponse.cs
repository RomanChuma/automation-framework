using System.Collections.Generic;

using Newtonsoft.Json;

namespace AutomationFramework.Core.Utils.TestRail.Models.GetCaseFields
{
	/// <summary>
	/// Data class for 'get_case_fields' API call
	/// http://docs.gurock.com/testrail-api2/reference-cases-fields
	/// </summary>
	public class GetCaseFieldsResponse
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("is_active")]
		public bool IsActive { get; set; }

		[JsonProperty("type_id")]
		public int TypeId { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("system_name")]
		public string SystemName { get; set; }

		[JsonProperty("label")]
		public string Label { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("configs")]
		public List<Config> Configs { get; set; }

		[JsonProperty("display_order")]
		public int DisplayOrder { get; set; }

		[JsonProperty("include_all")]
		public bool IncludeAll { get; set; }

		[JsonProperty("template_ids")]
		public List<object> TemplateIds { get; set; }
	}
}
