using AutomationFramework.Core.Configuration.Framework;
using AutomationFramework.Core.Configuration.Jenkins;
using AutomationFramework.Core.Configuration.TestRail;

namespace AutomationFramework.Core.Configuration
{
	/// <summary>
	/// Settings of the automation test run
	/// </summary>
	public static class GlobalSettings
	{
		static GlobalSettings()
		{
			TestRail = new TestRailSettings().Initialize() as TestRailSettings;
			Framework = new FrameworkSettings().Initialize() as FrameworkSettings;
			Jenkins = new JenkinsSettings().Initialize() as JenkinsSettings;
		}

		/// <summary>
		/// Gets automation framework settings
		/// </summary>
		public static FrameworkSettings Framework { get; }

		/// <summary>
		/// Gets Jenkins settings
		/// </summary>
		public static JenkinsSettings Jenkins { get; }

		/// <summary>
		/// Gets TestRail settings
		/// </summary>
		public static TestRailSettings TestRail { get; }
	}
}