using OpenQA.Selenium;

namespace AutomationFramework.Core.Engine
{
	public partial class Browser
	{
		/// <summary>
		/// Invoke javascript
		/// </summary>
		/// <param name="script">
		/// Script source
		/// </param>
		/// <param name="args">
		/// The args.
		/// </param>
		public static object InvokeScript(string script, params object[] args)
		{
			var javaScriptExecutor =
				Instance as IJavaScriptExecutor;
			var result = javaScriptExecutor.ExecuteScript(script, args);
			return result;
		}
	}
}
