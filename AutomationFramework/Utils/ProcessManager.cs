using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Enums;
using AutomationFramework.Core.Utils.Log;

namespace AutomationFramework.Core.Utils
{
	/// <summary>
	/// Helper class to manage system processes
	/// </summary>
	public static class ProcessManager
	{
		/// <summary>
		/// Dictionary containing the WebDriver and it's window process name
		/// </summary>
		private static readonly Dictionary<BrowserType, string> DriverProcesses = new Dictionary<BrowserType, string>
		                                                                          {
			                                                                          {
				                                                                          BrowserType.Chrome,
				                                                                          "chromedriver"
			                                                                          },
			                                                                          {
				                                                                          BrowserType.Firefox, "firefox"
			                                                                          },
			                                                                          {
				                                                                          BrowserType.InternetExplorer,
				                                                                          "IEDriverServer"
			                                                                          }
		                                                                          };

		private static readonly ILogger Log = Log4NetLogger.Instance;

		/// <summary>
		/// Kill system process by it's name
		/// </summary>
		/// <param name="processName">Process to kill</param>
		public static void KillProcessesByName(string processName)
		{
			var processes = Process.GetProcessesByName(processName);

			foreach (Process process in processes)
			{
				Log.Trace($"Killing '{process.ProcessName}' process");
				process.Kill();
			}
		}

		/// <summary>
		/// Kill the running webdriver processes
		/// </summary>
		public static void KillRunningDriverProcesses()
		{
			BrowserType browserType = GlobalSettings.Framework.Browser;
			string driverProcessName = DriverProcesses.FirstOrDefault(driver => driver.Key == browserType).Value;
			KillProcessesByName(driverProcessName);
		}
	}
}