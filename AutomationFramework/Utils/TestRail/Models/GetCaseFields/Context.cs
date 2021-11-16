using System.Collections.Generic;

using Newtonsoft.Json;

namespace AutomationFramework.Core.Utils.TestRail.Models.GetCaseFields
{
	public class Context
	{
		[JsonProperty("is_global")]
		public bool IsGlobal { get; set; }

		[JsonProperty("project_ids")]
		public List<object> ProjectIds { get; set; }
	}
}
