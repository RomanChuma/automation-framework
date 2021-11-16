using System.Collections.Generic;

using AutomationFramework.Core.Utils.TestRail.Models.GetStatus;

using Newtonsoft.Json;

namespace AutomationFramework.Core.Utils.TestRail.Endpoints
{
	/// <summary>
	/// API methods to request details about test statuses
	/// </summary>
	public class Statuses
	{
		/// <summary>
		/// Statuses : Passed, Blocked, Untested, Retest, Failed, TestAuto in progress ..., Not Tested In This version, Not Applicable, Defect, In Progress
		/// </summary>
		/// <returns>Returning an array of the JSON objects for each statuses (Example JSON : label="Passed", id=1, name="passed", is_final=true) </returns>
		public static List<GetStatusResponse> GetStatuses()
		{
			var response = TestRail.ApiClient.SendGet("get_statuses");
			var statuses = JsonConvert.DeserializeObject<List<GetStatusResponse>>(response.ToString());
			return statuses;
		}

		/// <summary>
		/// Get the id of a specific status (such as the id of status 'Passed')
		/// </summary>
		/// <param name="statusName">
		/// Passed, Blocked, Untested, Retest, etc.
		/// </param>
		/// <returns>
		/// Status id or '0' in case no match
		/// </returns>
		public int GetStatusIdByStatusName(string statusName)
		{
			int statusId = 0;

			var statuses = GetStatuses();

			foreach (var status in statuses)
			{
				if (status.Name.ToUpper() == statusName.ToUpper())
				{
					statusId = status.Id;
				}
			}

			return statusId;
		}
	}
}
