using System.ComponentModel;

namespace AutomationFramework.Core.Utils.TestRail
{
	/// <summary>
	/// TestRail test case type
	/// </summary>
	public enum TestCaseType
	{
		[Description("Automated")]
		Automated,
		[Description("Manual")]
		Manual,
		[Description("To be determined")]
		ToBeDetermined
	}
}
