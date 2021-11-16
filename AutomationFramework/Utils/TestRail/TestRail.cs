using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Extensions;
using AutomationFramework.Core.Utils.Log;
using AutomationFramework.Core.Utils.TestRail.Endpoints;
using AutomationFramework.Core.Utils.TestRail.Models.GetTests;

using Gurock.TestRail;

namespace AutomationFramework.Core.Utils.TestRail
{
	public sealed class TestRail
	{
		private static readonly object CheckLock = new object();

		private static readonly ILogger Log = Log4NetLogger.Instance;

		private static TestRail instance;

		private TestRail()
		{
			try
			{
				ApiClient = new APIClient(GlobalSettings.TestRail.Url)
				{
					User = GlobalSettings.TestRail.Username,
					Password = GlobalSettings.TestRail.Password
				};

				TestRunId = GlobalSettings.TestRail.TestRunId;
				
				TestsToRun = InitializeTestCasesToRun();
			}
			catch (Exception e)
			{
				Log.Error("Could not initialize test cases to run.", e);
				throw;
			}
		}

		public static APIClient ApiClient { get; set; }

		public static TestRail Instance
		{
			get
			{
				lock (CheckLock)
				{
					if (instance == null)
					{
						instance = new TestRail();
					}

					return instance;
				}
			}
		}

		public static int TestRunId { get; set; }

		/// <summary>
		/// Gets or sets automated tests that are matching the TestRail test run
		/// </summary>
		public static List<GetTestsResponse> TestsToRun { get; set; }

		/// <summary>
		/// Is test run id defined
		/// </summary>
		public bool IsTestRailExecution => (TestRunId != 0);

		/// <summary>
		/// Get the current test automation test run id from the active test run by manual test case id
		/// </summary>
		/// <param name="manualTestCaseId">Manual test case id, e.g. '1793'</param>
		/// <returns>Test id or 0 in case no match</returns>
		public int GetTestIdFromTestRunByCaseId(int manualTestCaseId)
		{
			bool manualTestCaseIdIsInvalid = manualTestCaseId <= 0;

			if (manualTestCaseIdIsInvalid)
			{
				throw new ArgumentOutOfRangeException(nameof(manualTestCaseId));
			}

			var testId = TestsToRun.Where(autoTest => autoTest.CaseId.Equals(manualTestCaseId)).Select(y => y.Id)
								   .FirstOrDefault();
			return testId;
		}

		/// <summary>
		/// Verify that automation test is present in TestRail test run
		/// </summary>
		/// <param name="caseId">TestRail test case Id</param>
		/// <returns>Boolean value</returns>
		public bool VerifyThatAutoTestPresentInTestRun(int caseId)
		{
			bool isCaseDefinedInTestRun = TestsToRun.Exists(test => test.CaseId.Equals(caseId));
			return isCaseDefinedInTestRun;
		}

		/// <summary>
		/// Get the list of test that are automated and that are present in the current test run
		/// </summary>
		/// <param name="testRunId">TestRail test run id</param>
		/// <returns>List of GetTestsResponse objects</returns>
		private List<GetTestsResponse> GetAutomatedTestsFromTestRun(int testRunId)
		{
			var results = new List<GetTestsResponse>();
			var caseTypeApi = new CaseTypes();
			int automatedTypeId = caseTypeApi.GetCaseTypeIdByName(TestCaseType.Automated);
			int completedTestStatusId = GetTestStatusIdByTestCaseStatusName(TestCaseStatus.Completed);
			var statusesApi = new Statuses();

			// TestAuto in progress ...
			var testAutoInProgressTestRunStatusName = "automated_tests_progress";
			int inProgressStatusId = statusesApi.GetStatusIdByStatusName(testAutoInProgressTestRunStatusName);

			var testsApi = new Tests();
			var testCasesFromTestRun = testsApi.GetTestCasesFromTestRunId(testRunId);

			foreach (var test in testCasesFromTestRun)
			{
				bool testStatusEqualsToInProgress = test.StatusId == inProgressStatusId;

				if (testStatusEqualsToInProgress)
				{
					var manualTestCase = TestCases.GetCase(test.CaseId);

					bool isManualTestInAutomationInProgressStatus = manualTestCase.TypeId == automatedTypeId;
					bool isManualTestStatusEqualToCompleted =
						manualTestCase.CustomTestCaseStatus == completedTestStatusId;
					int seleniumPlatformIndex = 2;
					bool isAutomatedUsingSelenium = manualTestCase.CustomAutoplatform == seleniumPlatformIndex;

					// If manual test from test run is in 'TestAuto in progress' status, has custom TC Status field equals to 'Completed'
					// and is automated using Selenium platform - we run the test
					if (isManualTestInAutomationInProgressStatus && isManualTestStatusEqualToCompleted
																 && isAutomatedUsingSelenium)
					{
						results.Add(test);
					}
				}
			}

			return results;
		}

		private string[] GetDropdownItems(string itemsString)
		{
			// Split the whole string with custom field dropdown items via delimiter
			string[] dropdownItems = itemsString.Split('\n');
			return dropdownItems;
		}

		/// <summary>
		/// Get test status id by status name
		/// </summary>
		/// <param name="status">Expected status</param>
		/// <returns>String with ID</returns>
		private int GetTestStatusIdByTestCaseStatusName(TestCaseStatus status)
		{
			string testStatusId = string.Empty;
			var customFields = CaseFields.GetCustomFields(ApiClient);

			foreach (var customField in customFields)
			{
				bool customFieldIsTestCaseStatus = customField.Label == "TC Status";

				if (customFieldIsTestCaseStatus)
				{
					var itemsString = customField.Configs[0].Options.Items;

					string[] dropdownItems = GetDropdownItems(itemsString);

					foreach (string dropdownItem in dropdownItems)
					{
						string[] item = SplitDropdownItemStringIntoIdAndText(dropdownItem);

						var itemId = item[0];
						var itemText = item[1];

						bool testStatusEqualsToManualTestStatus = itemText == status.GetDescription();

						if (testStatusEqualsToManualTestStatus)
						{
							testStatusId = itemId;
						}
					}
				}
			}

			return Convert.ToInt32(testStatusId);
		}

		private List<GetTestsResponse> InitializeTestCasesToRun()
		{
			if (IsTestRailExecution)
			{
				TestsToRun = GetAutomatedTestsFromTestRun(TestRunId);
			}

			return TestsToRun;
		}

		private string[] SplitDropdownItemStringIntoIdAndText(string dropdownItem)
		{
			string pattern = ", ";

			// Split item e.g. '1, First' into id and text
			string[] item = Regex.Split(dropdownItem, pattern);
			return item;
		}
	}
}