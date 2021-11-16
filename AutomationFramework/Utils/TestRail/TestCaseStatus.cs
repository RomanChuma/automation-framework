using System.ComponentModel;

namespace AutomationFramework.Core.Utils.TestRail
{
	/// <summary>
	/// TestRail manual test case status
	/// </summary>
	public enum TestCaseStatus
	{
		[Description("Draft")]
		Draft,
		[Description("To be reviewed")]
		ToBeReviewed,
		[Description("In writing")]
		InWriting,
		[Description("To Automate")]
		ToAutomate,
		[Description("In Development")]
		InDevelopment,
		[Description("Completed")]
		Completed,
		[Description("Deprecated")]
		Deprecated
	}
}
