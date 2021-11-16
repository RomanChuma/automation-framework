using System;

using AutomationFramework.Core.Utils.Log;
using AutomationFramework.Core.Utils.TestRail.Models.AddResult;

using Gurock.TestRail;

using Newtonsoft.Json;

namespace AutomationFramework.Core.Utils.TestRail.Endpoints
{
	/// <summary>
	/// API methods to request details about test results and to add new test results
	/// </summary>
	public class Results
	{
		private static readonly ILogger Log = Log4NetLogger.Instance;

		/// <summary>
		/// Add a new test result, comment or assign a test.
		/// </summary>
		/// <param name="testToUpdateResult">
		/// The test To Update Result.
		/// </param>
		/// <param name="requestBody">
		/// The request body.
		/// </param>
		/// <returns>
		/// If successful, this method returns the new test result using the same response format as get_results, but with a single result instead of a list of results
		/// </returns>
		public AddResultResponse AddResult(int testToUpdateResult, AddResultRequest requestBody)
		{
			if (testToUpdateResult <= 0)
			{
				var errorMessage =
					$"Test id '{testToUpdateResult}' is not a valid value! Verify that the automation test status is not already run on TestRail";
				Log.Error(errorMessage);
				throw new ArgumentException(errorMessage);
			}

			try
			{
				var apiResponse = TestRail.ApiClient.SendPost($"add_result/{testToUpdateResult}", requestBody);
				var addResultResponse = JsonConvert.DeserializeObject<AddResultResponse>(apiResponse.ToString());
				return addResultResponse;
			}
			catch (Exception e)
			{
				var errorMessage =
					$"Was not able to update the result for test with id '{testToUpdateResult}' due to exception:"
					+ Environment.NewLine + e.Message;
				Log.Error(errorMessage, e);
				throw new APIException(errorMessage);
			}
		}
	}
}
