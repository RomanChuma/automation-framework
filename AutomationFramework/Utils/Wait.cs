using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Controls;
using AutomationFramework.Core.Controls.Dialogs.NgDialog;
using AutomationFramework.Core.Engine;
using AutomationFramework.Core.Enums;
using AutomationFramework.Core.Utils.Log;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

using Polly;
using Polly.Retry;

using By = AutomationFramework.Core.Engine.By;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace AutomationFramework.Core.Utils
{
	public static class Wait
	{
		private static readonly ILogger Log = Log4NetLogger.Instance;

		private static readonly TimeSpan SleepTimeout = TimeSpan.FromMilliseconds(GlobalSettings.Framework.SleepTimeout);

		/// <summary>
		/// Gets Fluent wait for conditions
		/// </summary>
		public static DefaultWait<IWebDriver> ForElementFluently
		{
			get
			{
				var wait = new DefaultWait<IWebDriver>(Browser.Instance)
				{
					Timeout = TimeSpan.FromSeconds(GlobalSettings.Framework.FluentWaitTimeout),
					PollingInterval = TimeSpan.FromMilliseconds(GlobalSettings.Framework.PollingTimeout)
				};
				wait.IgnoreExceptionTypes(
					typeof(NoSuchElementException),
					typeof(ElementNotVisibleException),
					typeof(StaleElementReferenceException));
				return wait;
			}
		}

		/// <summary>
		/// Wait using Thread.Sleep
		/// </summary>
		/// <param name="timeout">Timeout in TimeSpan format</param>
		public static void For(TimeSpan timeout)
		{
			Log.Trace($"Thread sleep for {timeout.Seconds} sec {timeout.Milliseconds} ms...");
			Thread.Sleep(timeout);
		}

		/// <summary>
		/// Wait using Thread.Sleep
		/// </summary>
		/// <param name="timeout">Timeout in milliseconds</param>
		public static void For(int timeout = 20)
		{
			Log.Trace($"Thread sleep for {timeout} ms...");
			Thread.Sleep(timeout);
		}

		/// <summary>
		/// Wait for active JQuery to complete
		/// </summary>
		/// <param name="numberOfRequests">Number of active requests to be left, default to 0</param>
		/// <param name="timeOut">Timeout, seconds</param>
		public static void ForActiveJQueryRequestsCountEqualTo(int numberOfRequests = 0, int timeOut = 20)
		{
			bool isJqueryPresent = IsJQueryUsedOnThePage();

			if (isJqueryPresent)
			{
				Log.Trace(
					$"[Begin] Waiting for number of active JQuery requests to be equal to '{numberOfRequests}' on page '{Browser.CurrentUrl}'");
				var wait = new WebDriverWait(Browser.Instance, TimeSpan.FromSeconds(timeOut));

				try
				{
					wait.Until(
						driver => ((IJavaScriptExecutor)Browser.Instance).ExecuteScript($"return jQuery.active == {numberOfRequests}")
																		 .Equals(true));
				}
				catch (Exception e)
				{
					object actualRequestsCount = ((IJavaScriptExecutor)Browser.Instance).ExecuteScript("return jQuery.active");
					string errorMessage =
						$"Active JQuery requests count ({actualRequestsCount}) is not equal to expected ({numberOfRequests}) after waiting {wait.Timeout.Seconds} seconds.";
					Log.Error(errorMessage, e);
					throw;
				}

				Log.Trace($"[End] Active JQuery requests count is equal to '{numberOfRequests}'");
			}
			else
			{
				Log.Trace($"JQuery is not used on the page '{Browser.CurrentUrl}'");
			}
		}

		/// <summary>
		/// Wait for specific active JQuery request to be complete
		/// </summary>
		/// <param name="numberOfRequests">Number of active requests to be left, default to 0</param>
		/// <param name="timeOut">Timeout, seconds</param>
		public static void ForActiveJQueryRequestsGreaterThan(int numberOfRequests = 0, int timeOut = 25)
		{
			var jQueryRequestCount = new List<int>();
			var retryAttempts = 3;
			bool isJqueryPresent = IsJQueryUsedOnThePage();
			var wait = new WebDriverWait(
				Browser.Instance,
				TimeSpan.FromSeconds(timeOut));

			RetryPolicy waitPolicy = Policy.Handle<WebDriverTimeoutException>().Retry(
				retryAttempts,
				(exception, retryCount) =>
					{
						Log.Warn(
							$"Exception occured in ForActiveJQueryRequestsGreaterThan. Attempting retry number {retryCount}.",
							exception);
					});

			waitPolicy.Execute(
				() =>
					{
						if (isJqueryPresent)
						{
							object activeCount = ((IJavaScriptExecutor)Browser.Instance).ExecuteScript("return jQuery.active");
							var count = Convert.ToInt32(activeCount);
							jQueryRequestCount.Add(count);
							var totalRequests = jQueryRequestCount.Sum();
							Log.Debug($"Total number of jQuery requests are: {totalRequests}");
							wait.Until(
								driver => totalRequests >= numberOfRequests && (bool)((IJavaScriptExecutor)Browser.Instance)
										  .ExecuteScript("return jQuery.active == false"));
						}
					});
		}

		/// <summary>
		/// Wait for AngularJS pending requests to complete
		/// </summary>
		public static void ForAngularJsPendingRequestsToComplete()
		{
			try
			{
				bool isRootElementInjectorFound = IsRootElementInjectorFound();

				if (isRootElementInjectorFound)
				{
					Log.Trace($"[Begin] Waiting for AngularJS pending requests to complete on page '{Browser.CurrentUrl}'");
					var wait = new WebDriverWait(Browser.Instance, TimeSpan.FromSeconds(GlobalSettings.Framework.AngularJsTimeout));
					var angularWaitScript =
						"return window.angular.element(document.body).injector().get('$http').pendingRequests.length == 0";

					wait.Until(driver => ((IJavaScriptExecutor)Browser.Instance).ExecuteScript(angularWaitScript));
					var pendingRequestsScript =
						"return window.angular.element(document.body).injector().get('$http').pendingRequests.length";
					object pendingRequestsCount = Browser.InvokeScript(pendingRequestsScript);
					Log.Trace($"[End] AngularJS pending requests count is '{pendingRequestsCount}''");
				}
				else
				{
					Log.Trace($"Injector for root element is not defined on the page '{Browser.CurrentUrl}'");
				}
			}
			catch (Exception e)
			{
				string errorMessage =
					$"Wait for AngularJS pending requests on the page '{Browser.CurrentUrl}'{Environment.NewLine}was not completed due to exception:{Environment.NewLine}{e.Message}";
				Log.Error(errorMessage, e);
				throw new InvalidOperationException(errorMessage, e);
			}
		}

		/// <summary>
		/// Wait for AngularJS to be stable using Angular Testability API
		/// https://angular.io/api/core/Testability
		/// </summary>
		public static void ForAngularJsTestability()
		{
			try
			{
				bool isRootElementInjectorFound = IsRootElementInjectorFound();

				if (isRootElementInjectorFound)
				{
					Log.Trace($"[Begin] Waiting for AngularJS to return testability state on the page '{Browser.CurrentUrl}'");
					var wait = new WebDriverWait(Browser.Instance, TimeSpan.FromSeconds(GlobalSettings.Framework.AngularJsTimeout));
					string baseDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
					string angularWaitScriptPath = Path.Combine(baseDirectoryPath, "JS/AngularWaits/DeclareAngularTestability.js");
					string angularWaitScript = FileHandler.ReadFileContent(angularWaitScriptPath);
					((IJavaScriptExecutor)Browser.Instance).ExecuteScript(angularWaitScript);

					string waitDeclarationFilePath = Path.Combine(baseDirectoryPath, "JS/AngularWaits/WaitForAngularTestability.js");
					string waitDeclarationScript = FileHandler.ReadFileContent(waitDeclarationFilePath);
					((IJavaScriptExecutor)Browser.Instance).ExecuteScript(waitDeclarationScript);

					string angularTestabilityResultScriptPath = Path.Combine(
						baseDirectoryPath,
						"JS/AngularWaits/AngularTestabilityResult.js");
					string angularTestabilityResultScript = FileHandler.ReadFileContent(angularTestabilityResultScriptPath);
					wait.Until(driver => ((IJavaScriptExecutor)Browser.Instance).ExecuteScript(angularTestabilityResultScript));
					Log.Trace("[End] AngularJS is testable");
				}
				else
				{
					Log.Trace($"No injector found for element argument to getTestability on page '{Browser.CurrentUrl}'");
				}
			}
			catch (Exception e)
			{
				string errorMessage =
					$"Wait for AngularJS testability state on the page '{Browser.CurrentUrl}'{Environment.NewLine}was not completed due to exception:{Environment.NewLine}{e.Message}";
				Log.Error(errorMessage, e);
				throw new InvalidOperationException(errorMessage, e);
			}
		}

		/// <summary>
		/// Wait for pending AngularJS requests to be completed and for testability state to be true
		/// </summary>
		public static void ForAngularToComplete()
		{
			bool isAngularPage = IsAngularUsedOnThePage();

			if (isAngularPage)
			{
				ForAngularJsPendingRequestsToComplete();
				ForAngularJsTestability();
			}
			else
			{
				Log.Trace($"'{Browser.CurrentUrl}' page does not use AngularJS");
			}
		}

		public static void ForDialogToBeClosed(NgDialogElement dialog)
		{
			Log.Trace("Waiting for dialog to be closed");

			Until(() => dialog == null, TimeSpan.FromSeconds(GlobalSettings.Framework.DialogCloseTimeout));
		}

		/// <summary>
		/// Wait for the NgDialog to open
		/// </summary>
		/// <param name="dialog">Dialog</param>
		public static void ForDialogToOpen(NgDialogElement dialog)
		{
			Log.Trace("Waiting for dialog to open");

			Until(() => dialog != null, TimeSpan.FromSeconds(GlobalSettings.Framework.DialogOpenTimeout));
		}

		/// <summary>
		/// Wait for file of given type to appear in the target folder
		/// </summary>
		/// <param name="folderPath">Folder path</param>
		/// <param name="fileName">File type</param>
		/// <param name="timeout">Wait timeout</param>
		public static void ForFileToBePresentInFolder(string folderPath, string fileName, TimeSpan? timeout = null)
		{
			if (timeout == null)
			{
				timeout = TimeSpan.FromSeconds(GlobalSettings.Framework.FileDownloadTimeout);
			}

			try
			{
				Log.Trace($"[Begin] Waiting for file '{fileName}' to be present in folder '{folderPath}'");

				Until(() => FileHandler.FileIsPresentInFolder(folderPath, fileName), timeout);

				Log.Trace($"[End] File '{fileName}' is present in folder '{folderPath}'");
			}
			catch (Exception e)
			{
				var fileList = FileHandler.GetFilesByPattern(folderPath);
				var fileNames = fileList.Select(file => file.Name).ToList();
				string delimiter = Environment.NewLine;
				string logMessage = $"File '{fileName}' is not present in folder '{folderPath}'." + Environment.NewLine
																								  + "List of files are present in folder:"
																								  + Environment.NewLine
																								  + fileNames.Aggregate(
																									  (i, j) => i + delimiter + j);
				Log.Error(logMessage, e);
				throw;
			}
		}

		/// <summary>
		/// Wait for file of given type to appear in the default folder
		/// </summary>
		/// <param name="fileName">File type</param>
		/// <param name="timeout">Wait timeout</param>
		public static void ForFileToBePresentInFolder(string fileName, TimeSpan? timeout = null)
		{
			var folderPath = Environment.ExpandEnvironmentVariables(GlobalSettings.Framework.DownloadsFolder);
			ForFileToBePresentInFolder(folderPath, fileName, timeout);
		}

		/// <summary>
		/// Wait for file of given type to appear in the target folder
		/// </summary>
		/// <param name="folderPath">Folder path</param>
		/// <param name="fileType">File type</param>
		/// <param name="timeout">Wait timeout</param>
		public static void ForFileToBePresentInFolder(string folderPath, FileType fileType, TimeSpan? timeout = null)
		{
			if (timeout == null)
			{
				timeout = TimeSpan.FromSeconds(GlobalSettings.Framework.FileDownloadTimeout);
			}

			Log.Trace($"[Begin] Waiting for file '{fileType}' to be present in folder '{folderPath}'");
			Until(() => FileHandler.FileIsPresentInFolder(folderPath, fileType), timeout);
			Log.Trace($"[End] File '{fileType}' is present in folder '{folderPath}'");
		}

		/// <summary>
		/// Wait for loading indicator spinner to disappear
		/// </summary>
		public static void ForLoadingIndicatorToDisappear()
		{
			Log.Trace($"[Begin] Waiting for loading indicator to disappear from '{Browser.Title}' page");
			var wait = new WebDriverWait(Browser.Instance, TimeSpan.FromSeconds(GlobalSettings.Framework.LoadingIndicatorTimeout));
			wait.Until(driver => Browser.FindElements<UiElement>(By.CssSelector(".waiting, .tb-loading")).Count == 0);
			Log.Trace("[End] Loading indicator disappeared");
		}

		/// <summary>
		/// Wait for document.readyState status on the page to be completed
		/// </summary>
		public static void ForPageReadyStateToComplete()
		{
			var retryAttempts = 3;

			RetryPolicy waitPolicy = Policy.Handle<WebDriverTimeoutException>().Retry(
				retryAttempts,
				(exception, retryCount) =>
					{
						Log.Warn(
							$"Exception occured on Waiting for ASP.NET 'document.readyState' to be 'complete'. Attempting retry number {retryCount}.",
							exception);
					});

			waitPolicy.Execute(
				() =>
					{
						Log.Trace($"[Begin] Waiting for ASP.NET 'document.readyState' to be 'complete' on '{Browser.CurrentUrl}' page");

						var wait = new WebDriverWait(Browser.Instance, TimeSpan.FromSeconds(GlobalSettings.Framework.ReadyStateTimeout));
						wait.Until(
							driver => ((IJavaScriptExecutor)Browser.Instance).ExecuteScript("return document.readyState")
																			 .Equals("complete"));

						var documentReadyStateScript = "return document.readyState";
						object currentDocumentReadyState = Browser.InvokeScript(documentReadyStateScript);

						Log.Trace($"[End] 'document.readyState' is '{currentDocumentReadyState}'");
					});
		}

		/// <summary>
		/// Wait for URL to be opened, which contains specific string
		/// </summary>
		/// <param name="expectedUrl">Expected URL</param>
		/// <param name="timeOut">Timeout, seconds</param>
		public static void ForPartUrlToBeOpened(string expectedUrl, int timeOut = 0)
		{
			Log.Trace($"Waiting for page '{expectedUrl}'...");

			if (timeOut == 0)
			{
				timeOut = GlobalSettings.Framework.UrlOpenTimeout;
			}

			try
			{
				var wait = new WebDriverWait(Browser.Instance, TimeSpan.FromSeconds(timeOut));
				wait.Until(ExpectedConditions.UrlContains(expectedUrl.ToLower()));
			}
			catch (TimeoutException e)
			{
				string errorMessage = $"Can not load the '{expectedUrl}' due to a timeout. Make sure that application is up";
				Log.Error(errorMessage, e);
				throw new TimeoutException(errorMessage, e);
			}

			Log.Trace($"'{expectedUrl}' is opened");
		}

		/// <summary>
		/// Wait for progress message box to disappear
		/// </summary>
		public static void ForProgressMessageBoxNotToBeVisible()
		{
			Log.Trace($"[Begin] Waiting for progress message box to disappear from '{Browser.Title}' page");

			var wait = new WebDriverWait(Browser.Instance, TimeSpan.FromSeconds(GlobalSettings.Framework.DialogOpenTimeout));
			wait.Until(driver => !Browser.FindElement<UiElement>(By.XPath("//div[@aria-describedby='messageBoxProgress']")).IsVisible);
			Log.Trace("[End] Progress message box disappeared");
		}

		/// <summary>
		/// Wait for URL to be opened
		/// </summary>
		/// <param name="expectedUrl">Expected URL</param>
		/// <param name="timeOut">Timeout, seconds</param>
		public static void ForUrlToBeOpened(string expectedUrl, int timeOut = 0)
		{
			Log.Trace($"[Begin] Waiting for page '{expectedUrl}'...");

			if (timeOut == 0)
			{
				timeOut = GlobalSettings.Framework.UrlOpenTimeout;
			}

			var wait = new WebDriverWait(Browser.Instance, TimeSpan.FromSeconds(timeOut));
			wait.Until(ExpectedConditions.UrlToBe(expectedUrl.ToLower()));
			Log.Trace($"[End] '{expectedUrl}' url is opened");
		}

		/// <summary>
		/// Wait for a condition with given timeout
		/// </summary>
		/// <param name="action">The condition to be met</param>
		/// <param name="timeout">Timeout, in TimeSpan format. Default to 20 seconds.</param>
		public static void Until(Func<bool> action, TimeSpan? timeout = null)
		{
			if (timeout == null)
			{
				timeout = TimeSpan.FromSeconds(GlobalSettings.Framework.ActionWaitTimeout);
			}

			DateTime timeoutDate = DateTime.Now.Add(timeout.Value);
			bool isActionCompleted;

			Log.Trace($"[Begin] Waiting until {action.Method.Name} is completed or exit by timeout after {timeout}");

			do
			{
				isActionCompleted = action();

				if (!isActionCompleted)
				{
					For(SleepTimeout);
				}
			}
			while (!isActionCompleted && DateTime.Now < timeoutDate);

			if (!isActionCompleted)
			{
				string errorMessage = $"Wait for {action.Method.Name} exited by timeout of {timeout}";
				Log.Error(errorMessage);

				throw new TimeoutException(errorMessage);
			}

			Log.Trace($"[End] Wait for {action.Method.Name} is completed");
		}

		private static bool IsAngularUsedOnThePage()
		{
			var script = @"if (window.angular){
        return true;
        }";
			var isAngularUsed = Convert.ToBoolean(Browser.InvokeScript(script));
			return isAngularUsed;
		}

		private static bool IsJQueryUsedOnThePage()
		{
			var script = "return window.jQuery != undefined";
			var isJqueryUsed = Convert.ToBoolean(Browser.InvokeScript(script));
			return isJqueryUsed;
		}

		/// <summary>
		/// Return result is injector found for element argument to getTestability
		/// See https://code.angularjs.org/1.4.8/docs/error/ng/test
		/// </summary>
		/// <returns>Is injector found</returns>
		private static bool IsRootElementInjectorFound()
		{
			var script = "return window.angular.element(document.body).injector() != undefined";
			var isInjectorFound = Convert.ToBoolean(Browser.InvokeScript(script));
			return isInjectorFound;
		}
	}
}