using System.Collections.Generic;

using AutomationFramework.Core.Utils.TestRail.Models.GetCaseFields;

using Gurock.TestRail;

using Newtonsoft.Json;

namespace AutomationFramework.Core.Utils.TestRail.Endpoints
{
	/// <summary>
	///  API methods to request details about custom fields for test cases
	/// </summary>
	public static class CaseFields
	{
		/// <summary>
		/// Get a list of available test case custom fields
		/// </summary>
		/// <param name="testRailApiClient">TestRail API client</param>
		/// <returns>List of GetCaseFieldsResponse objects</returns>
		public static List<GetCaseFieldsResponse> GetCustomFields(APIClient testRailApiClient)
		{
			var apiResponse = testRailApiClient.SendGet("get_case_fields");
			var getCustomFieldsResponse = JsonConvert.DeserializeObject<List<GetCaseFieldsResponse>>(apiResponse.ToString());
			return getCustomFieldsResponse;
		}
	}
}
