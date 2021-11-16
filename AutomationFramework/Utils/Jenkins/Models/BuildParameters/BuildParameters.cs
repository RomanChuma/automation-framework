using System.Web;

using Newtonsoft.Json;

namespace AutomationFramework.Core.Utils.Jenkins.Models.BuildParameters
{
	public class BuildParameters
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("value")]
		public string Value { get; set; }

		public string ToKeyValue() => $"{HttpUtility.UrlEncode(Name)}={HttpUtility.UrlEncode(Value)}";
	}
}