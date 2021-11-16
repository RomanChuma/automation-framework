using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;

using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Controls;
using AutomationFramework.Core.Enums;
using AutomationFramework.Core.Utils;
using AutomationFramework.Core.Utils.Log;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Extensions;

using Polly;
using Polly.Retry;

namespace AutomationFramework.Core.Engine
{
	public partial class Browser
	{
		/// <summary>
		/// Storage of <see cref="EventFiringBrowser"/> wrappers around the default <see cref="IWebDriver"/>
		/// </summary>
		private static readonly ThreadLocal<IWebDriver> EventFiringDrivers = new ThreadLocal<IWebDriver>();

		private static readonly ILogger Log = Log4NetLogger.Instance;

		/// <summary>
		/// Storage of browser drivers
		/// </summary>
		private static readonly ThreadLocal<IWebDriver> WebDrivers = new ThreadLocal<IWebDriver>();

		private Browser()
		{
		}

		/// <summary>
		/// Browser URL
		/// </summary>
		public static string CurrentUrl
		{
			get
			{
				string url;
				try
				{
					url = Instance.Url;
				}
				catch (NoSuchWindowException)
				{
					SwitchToParentWindow();
					url = Instance.Url;
				}

				return url;
			}
		}

		/// <summary>
		/// Browser HTML source
		/// </summary>
		public static string HtmlSource => Instance.PageSource;

		/// <summary>
		/// Gets or sets webDriver storage for parallelization in a single machine
		/// </summary>
		public static IWebDriver Instance
		{
			get
			{
				if (EventFiringDrivers.Value == null)
				{
					Log.Warn("Browser is not started. Please call method 'Start' before can get Driver");
				}

				return EventFiringDrivers.Value;
			}

			set => EventFiringDrivers.Value = value;
		}

		/// <summary>
		/// Gets browser name
		/// </summary>
		public static string Name => Capabilities.GetCapability("browserName").ToString();

		/// <summary>
		/// Browser title
		/// </summary>
		public static string Title => Instance.Title;

		/// <summary>
		/// Gets browser version
		/// </summary>
		public static string Version
		{
			get
			{
				object version = Capabilities.GetCapability("version");

				// In some case, version attribute doesn't exists, so we need the browserVersion attribute.
				// This is due to computer configuration.
				if (version == null)
				{
					version = Capabilities.GetCapability("browserVersion");
				}

				return version.ToString();
			}
		}

		private static ICapabilities Capabilities
		{
			get
			{
				if (Instance != null)
				{
					ICapabilities capabilities = ((RemoteWebDriver)NativeDriverInstance).Capabilities;
					return capabilities;
				}

				return null;
			}
		}

		/// <summary>
		/// Gets or sets webDriver storage for parallelization in a single machine
		/// </summary>
		private static IWebDriver NativeDriverInstance
		{
			get
			{
				if (WebDrivers.Value == null)
				{
					Log.Warn("Please call method 'Start' before can get Driver");
				}

				return WebDrivers.Value;
			}

			set => WebDrivers.Value = value;
		}

		private static ScreenshotImageFormat ScreenshotImageFormat => ScreenshotImageFormat.Jpeg;

		/// <summary>
		/// Close the browser window
		/// </summary>
		public static void Close()
		{
			Instance.Close();
			Log.Info($"Closed the {Name} instance");
		}

		/// <summary>
		/// Check if scrollbar is present on the page (browser) level
		/// </summary>
		/// <param name="scrollBarType">Scroll bar type</param>
		/// <returns>Bool</returns>
		public static bool IsPageScrollbarPresent(ScrollBarType scrollBarType)
		{
			var isPageScrollBarPresent = false;

			switch (scrollBarType)
			{
				case ScrollBarType.Horizontal:
					isPageScrollBarPresent = Convert.ToBoolean(
						InvokeScript("return document.documentElement.scrollWidth>document.documentElement.clientWidth;"));
					break;

				case ScrollBarType.Vertical:
					isPageScrollBarPresent = Convert.ToBoolean(
						InvokeScript("return document.documentElement.scrollHeight>document.documentElement.clientHeight;"));
					break;
			}

			return isPageScrollBarPresent;
		}

		/// <summary>
		/// Check if URL end matches the specified string
		/// </summary>
		/// <param name="expectedUrlEndPart">Expected URL end</param>
		/// <returns>bool</returns>
		public static bool IsUrlEndsWith(string expectedUrlEndPart) => Instance.Url.EndsWith(expectedUrlEndPart);

		/// <summary>
		/// Navigate to specified URL
		/// </summary>
		/// <param name="url">URL to navigate</param>
		public static void NavigateTo(string url)
		{
			try
			{
				var retryAttempts = 5;

				RetryPolicy navigationPolicy = Policy.Handle<WebDriverTimeoutException>().Retry(
					retryAttempts,
					(exception, retryCount) =>
						{
							Log.Warn($"Navigation to '{url}' failed. Attempting retry number {retryCount}.", exception);
						});

				Log.Debug($"Navigating to '{url}' URL");
				navigationPolicy.Execute(
					() =>
						{
							NativeDriverInstance.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(GlobalSettings.Framework.UrlOpenTimeout);
							Instance.Navigate().GoToUrl(url);
							NativeDriverInstance.Manage().Timeouts().ImplicitWait =
								TimeSpan.FromSeconds(GlobalSettings.Framework.ImplicitWaitTimeout);
						});
			}
			catch (WebDriverTimeoutException ex)
			{
				Log.Warn($"Navigation to {url} URL exited by timeout");
				Log.Error(ex.Message, ex);
				Log.Error(ex.StackTrace, ex);
				throw;
			}
			catch (Exception ex)
			{
				Log.Warn($"Exception occured on navigating to '{url}' URL");
				Log.Error(ex.Message, ex);
				Log.Error(ex.StackTrace, ex);
				throw;
			}
		}

		/// <summary>
		/// Open new window and switch on it.
		/// </summary>
		public static void OpenNewWindow()
		{
			((IJavaScriptExecutor)Instance).ExecuteScript("window.open();");
			SwitchToLatestBrowserWindow();
		}

		/// <summary>
		/// Quit driver
		/// </summary>
		public static void Quit()
		{
			Instance.Quit();
			Log.Info($"Quit the {Name} instance");
		}

		/// <summary>
		/// Refresh browser
		/// </summary>
		public static void Refresh()
		{
			Instance.Navigate().Refresh();

			Wait.ForPageReadyStateToComplete();
			Wait.ForAngularToComplete();
		}

		/// <summary>
		/// Scroll to page by X pixel horizontally and Y pixel vertically
		/// </summary>
		/// <param name="x">amount of pixel horizontally</param>
		/// <param name="y">amount of pixel vertically</param>
		public static void ScrollBy(int x, int y)
		{
			InvokeScript($"scrollBy({x}, {y})");
		}

		/// <summary>
		/// Start the browser instance
		/// </summary>
		public static void Start()
		{
			try
			{
				// Location of the driver executable, since we're adding the driver via Nuget, they're placed in the bin directory
				string driverDirectory = AppDomain.CurrentDomain.BaseDirectory;

				switch (GlobalSettings.Framework.Browser)
				{
					case BrowserType.Chrome:
						ChromeOptions chromeOptions = ChromeProfile;
						chromeOptions.PageLoadStrategy = PageLoadStrategy.Normal;
						ChromeDriverService chromeService = ConfigureChromeDriverService(driverDirectory);

						TimeSpan browserCommandTimeout = TimeSpan.FromMinutes(GlobalSettings.Framework.BrowserCommandTimeout);
						NativeDriverInstance = new ChromeDriver(chromeService, chromeOptions, browserCommandTimeout);
						break;
					case BrowserType.Firefox:
					case BrowserType.InternetExplorer:
					case BrowserType.Edge:
						throw new NotSupportedException(
							$"{GlobalSettings.Framework.Browser} browser support is not yet added to the framework");
				}

				EventFiringDrivers.Value = new EventFiringBrowser(NativeDriverInstance);

				Log.Info($"Started {Name} browser instance");
				Log.Debug($"{GlobalSettings.Framework.Browser} browser version is: {Version}");

				NativeDriverInstance.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(GlobalSettings.Framework.ImplicitWaitTimeout);
				NativeDriverInstance.Manage().Timeouts().AsynchronousJavaScript =
					TimeSpan.FromSeconds(GlobalSettings.Framework.AsyncJsTimeout);
				NativeDriverInstance.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(GlobalSettings.Framework.PageLoadTimeout);

				DeleteAllCookies();
			}
			catch (Exception e)
			{
				Log.Warn($"Exception detected when try to launch new {GlobalSettings.Framework.Browser} webdriver instance.");
				Log.Error(e.Message, e);
				Log.Error(e.StackTrace, e);
				throw;
			}
		}

		/// <summary>
		/// Switch browser back from iframe to default content
		/// </summary>
		public static void SwitchFromIframeToDefaultContent() => Instance.SwitchTo().DefaultContent();

		/// <summary>
		/// Switch to the specific browser window by Index. Latest browser window will be the last in the list
		/// Will switch to the latest browser window opened, if no index is provided.
		/// </summary>
		/// <param name="browserWindowIndex">Index of the desired browser window. -1 is set by default to select the latest
		/// browser window.</param>
		public static void SwitchToBrowserWindow(int browserWindowIndex)
		{
			var openedWindows = GetListOfOpenedBrowserWindows();

			Instance.SwitchTo().Window(openedWindows[browserWindowIndex]);
		}

		/// <summary>
		/// Switch to default browser window
		/// </summary>
		public static void SwitchToDefaultBrowserWindow()
		{
			const int DefaultBrowserWindowIndex = 0;
			SwitchToBrowserWindow(DefaultBrowserWindowIndex);
		}

		/// <summary>
		/// Switch browser to the frame on the page
		/// </summary>
		/// <param name="id">
		/// IFrame ID
		/// </param>
		public static void SwitchToIframeById(string id)
		{
			var iframeList = GetIframes();
			bool isIframePresent = iframeList.Exists(x => x.GetAttribute("id").Equals(id));

			if (isIframePresent)
			{
				Instance.SwitchTo().Frame(id);
			}
			else
			{
				Log.Debug("Perform switching to the default content.");
				Instance.SwitchTo().DefaultContent();
				Log.Debug($"Trying to switch to frame '{id}' again.");
				Instance.SwitchTo().Frame(id);
			}
		}

		public static void SwitchToIframeByXpath(string xpath)
		{
			IWebElement frame = Instance.FindElement(OpenQA.Selenium.By.XPath(xpath));
			Instance.SwitchTo().Frame(frame);
		}

		public static void SwitchToLatestBrowserWindow()
		{
			var openedWindows = GetListOfOpenedBrowserWindows();
			int latestBrowserWindowIndex = openedWindows.Count - 1;
			SwitchToBrowserWindow(latestBrowserWindowIndex);
		}

		/// <summary>
		/// Switch to newly opened Tab
		/// </summary>
		public static void SwitchToNewTab()
		{
			Log.Trace($"Window Handles Count: {Instance.WindowHandles.Count}");

			string parentWindow = Instance.CurrentWindowHandle;

			foreach (string windowHandle in EventFiringDrivers.Value.WindowHandles)
			{
				if (!windowHandle.Equals(parentWindow))
				{
					Instance.SwitchTo().Window(windowHandle);
				}
			}
		}

		/// <summary>
		/// Switch to newly opened Tab
		/// </summary>
		public static void SwitchToParentWindow()
		{
			string parentWindowHandle = Instance.WindowHandles.First();
			Instance.SwitchTo().Window(parentWindowHandle);
		}

		/// <summary>
		/// Take screenshot of active browser
		/// </summary>
		/// <param name="screenshotFileName">
		/// The screenshot File Name.
		/// </param>
		/// <returns>
		/// Path to the created screenshot
		/// </returns>
		public static string TakeBrowserScreenshot(string screenshotFileName)
		{
			var retryAttempts = 3;

			RetryPolicy screenshotPolicy = Policy.Handle<WebDriverTimeoutException>().Retry(
				retryAttempts,
				(exception, retryCount) =>
					{
						Log.Warn($"Exception occured on taking browser screenshot. Attempting retry number {retryCount}.", exception);
					});

			string screenshotPath = FileHandler.CreateFolder(
				FileHandler.ProjectPath,
				ConfigurationManager.AppSettings["ScreenshotsDirectory"]);
			string fileName = $"{screenshotFileName}.{ScreenshotImageFormat}";
			string filenameWithPath = Path.Combine(screenshotPath, fileName);

			screenshotPolicy.Execute(
				() =>
					{
						try
						{
							Log.Trace("Taking a browser screenshot");

							Screenshot screenshot = Instance.TakeScreenshot();
							screenshot.SaveAsFile(filenameWithPath, ScreenshotImageFormat);
							Log.Debug($"Screenshot '{fileName}' can be found at '{screenshotPath}'");
						}
						catch (Exception ex)
						{
							Log.Warn($"Exception occured on taking browser screenshot\nDetails: {ex.Message}");
							throw;
						}
					});

			return filenameWithPath;
		}

		private static ChromeDriverService ConfigureChromeDriverService(string driverDirectory)
		{
			var chromeService = ChromeDriverService.CreateDefaultService(driverDirectory);
			chromeService.Port = NetworkHelper.GetFreeTcpPort();

			Log.Trace($"Chrome browser would run on TCP port '{chromeService.Port}'");
			return chromeService;
		}

		/// <summary>
		/// Return the list of iFrame control
		/// </summary>
		/// <returns></returns>
		private static List<UiElement> GetIframes() => FindElements<UiElement>(By.Tag("iframe"));

		/// <summary>
		/// Returns list of opened browser windows
		/// </summary>
		private static List<string> GetListOfOpenedBrowserWindows() => Instance.WindowHandles.ToList();
	}
}