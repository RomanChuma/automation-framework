namespace AutomationFramework.Core.Utils.TestRail
{
	/// <summary>
	/// Test status in TestRail
	/// </summary>
	public enum TestRailTestStatus
	{
		Passed = 1,
		Blocked = 2,
		Untested = 3,
		Retest = 4,
		Failed = 5,
		TestautoInProgress = 6,
		NoTestedInThisVersion = 7,
		NotApplicable = 8,
		Defect = 9,
		InProgress = 10
	}
}
