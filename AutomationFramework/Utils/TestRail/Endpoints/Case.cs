using AutomationFramework.Core.Utils.TestRail.Models.GetCase;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AutomationFramework.Core.Utils.TestRail.Endpoints
{
	/// <summary>
	/// API methods to request details about test cases and to create or modify test cases.
	/// </summary>
	public static class TestCases
	{
		/// <summary>
		/// Get an existing test case
		/// </summary>
		/// <param name="caseId">Test case ID</param>
		/// <returns>GetCaseResponse object</returns>
		public static GetCaseResponse GetCase(int caseId)
		{
			var apiResponse = TestRail.ApiClient.SendGet("get_case/" + caseId);
			var testcase = JsonConvert.DeserializeObject<GetCaseResponse>(apiResponse.ToString());
			return testcase;
		}

		public static JObject GetCases(string projectId)
		{
			var getCasesResponse = (JObject)TestRail.ApiClient.SendGet("get_cases/" + projectId);
			return getCasesResponse;
		}
	}
}
