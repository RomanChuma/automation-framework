using System.Collections.Generic;
using System.Net;

using AutomationFramework.Core.Extensions;
using AutomationFramework.Core.Utils.TestRail.Models.GetCaseTypes;

using Newtonsoft.Json;

namespace AutomationFramework.Core.Utils.TestRail.Endpoints
{
	/// <summary>
	/// API methods to request details about case type 
	/// </summary>
	public class CaseTypes
	{
		/// <summary>
		/// Get a list of available case types
		/// Types : To be determined, Manual, Automated
		/// </summary>
		/// <returns>Returning an array of the JSON objects for each types (Example JSON : name="To be determined", id=3, is_default=true) </returns>
		public List<GetCaseTypesResponse> GetCaseTypes()
		{
			// Following line of code is needed in order to establish secure connection between the VM with test code and TestRail API
			ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
			var apiResponse = TestRail.ApiClient.SendGet("get_case_types");
			var caseTypes = JsonConvert.DeserializeObject<List<GetCaseTypesResponse>>(apiResponse.ToString());
			return caseTypes;
		}

		/// <summary>
		/// Get the id of a specific type (such as the id of type 'Automated')
		/// </summary>
		/// <param name="type">
		/// To be determined, Automated, Manual
		/// </param>
		/// <returns>
		/// TestRail testcase type id or 0 in case no match
		/// </returns>
		public int GetCaseTypeIdByName(TestCaseType type)
		{
			int typeId = 0;

			var caseTypes = GetCaseTypes();

			foreach (var caseType in caseTypes)
			{
				if (caseType.Name == type.GetDescription())
				{
					typeId = caseType.Id;
				}
			}

			return typeId;
		}
	}
}
