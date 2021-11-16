using System;
using System.Collections.Generic;

using AutomationFramework.Core.Utils.Log;
using AutomationFramework.Core.Utils.TestRail.Models.GetTests;

using Newtonsoft.Json;

namespace AutomationFramework.Core.Utils.TestRail.Endpoints
{
	/// <summary>
	/// API methods to request details about tests
	/// </summary>
	public class Tests
	{
		private static readonly ILogger Log = Log4NetLogger.Instance;

		/// <summary>
		/// Get list of tests for a test run
		/// </summary>
		/// <param name="runId">
		/// The ID of the test run
		/// </param>
		/// <returns>
		/// List of test cases
		/// </returns>
		public List<GetTestsResponse> GetTestCasesFromTestRunId(int runId)
		{
			try
			{
				var apiResponse = TestRail.ApiClient.SendGet($"get_tests/{runId}");
				var testsPresentInTestRun =
					JsonConvert.DeserializeObject<List<GetTestsResponse>>(apiResponse.ToString());
				return testsPresentInTestRun;
			}
			catch (Exception e)
			{
				var errorMessage =
					$"Wasn't able to get the list of tests from the TestRail test run with id '{runId}' due to exception:"
					+ Environment.NewLine + e.Message;
				Log.Error(errorMessage, e);
				throw;
			}
		}
	}
}
