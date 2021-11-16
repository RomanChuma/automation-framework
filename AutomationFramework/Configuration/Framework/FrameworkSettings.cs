using System;
using System.Configuration;

using AutomationFramework.Core.Enums;
using AutomationFramework.Core.Utils.Log;

namespace AutomationFramework.Core.Configuration.Framework
{
	public class FrameworkSettings : ConfigurationSection, IConfiguration
	{
		private static readonly ILogger Log = Log4NetLogger.Instance;

		/// <summary>
		/// Timeout for actions to be completed, value in seconds
		/// </summary>
		[ConfigurationProperty("ActionWaitTimeout", DefaultValue = 30, IsRequired = false)]
		public int ActionWaitTimeout => (int)this["ActionWaitTimeout"];

		/// <summary>
		/// Timeout value for AngularJS testability state used in wait methods, value in seconds
		/// </summary>
		[ConfigurationProperty("AngularJsTimeout", DefaultValue = 60, IsRequired = false)]
		public int AngularJsTimeout => (int)this["AngularJsTimeout"];

		/// <summary>
		/// Timeout for asynchronous javascript execution, value in seconds
		/// </summary>
		[ConfigurationProperty("AsyncJsTimeout", DefaultValue = 60, IsRequired = false)]
		public int AsyncJsTimeout => (int)this["AsyncJsTimeout"];

		/// <summary>
		/// Runtime browser type
		/// </summary>
		public BrowserType Browser
		{
			get
			{
				bool isBrowserSupported = Enum.TryParse(BrowserFromAppConfig, out BrowserType browserType);

				if (!isBrowserSupported)
				{
					return BrowserType.None;
				}

				Log.Debug($"Browser value from App.config: {browserType}");
				return browserType;
			}
		}

		/// <summary>
		/// Timeout for browser commands, value in seconds
		/// </summary>
		[ConfigurationProperty("BrowserCommandTimeout", DefaultValue = 120, IsRequired = false)]
		public int BrowserCommandTimeout => (int)this["BrowserCommandTimeout"];

		/// <summary>
		/// Information for the database server
		/// </summary>
		[ConfigurationProperty("DatabaseServer", DefaultValue = "testautoserver.cch.ca", IsRequired = false)]
		public string DatabaseServer => (string)this["DatabaseServer"];

		/// <summary>
		/// Delete recorder videos after test finishes or not
		/// </summary>
		[ConfigurationProperty("DeleteVideoRecordsInCleanup", IsRequired = false)]
		public bool DeleteVideoRecordsInCleanup => (bool)this["DeleteVideoRecordsInCleanup"];

		/// <summary>
		/// Timeout for dialog to be closed, value in seconds
		/// </summary>
		[ConfigurationProperty("DialogCloseTimeout", DefaultValue = 30, IsRequired = false)]
		public int DialogCloseTimeout => (int)this["DialogCloseTimeout"];

		/// <summary>
		/// Timeout for dialog to be opened, value in seconds
		/// </summary>
		[ConfigurationProperty("DialogOpenTimeout", DefaultValue = 30, IsRequired = false)]
		public int DialogOpenTimeout => (int)this["DialogOpenTimeout"];

		/// <summary>
		/// Downloads directory
		/// </summary>
		[ConfigurationProperty("DownloadsFolder", DefaultValue = @"%userprofile%\Downloads", IsRequired = false)]
		public string DownloadsFolder
		{
			get
			{
				var appConfigValue = (string)this["DownloadsFolder"];
				string downloadsFolder = Environment.ExpandEnvironmentVariables(appConfigValue);
				return downloadsFolder;
			}
		}

		/// <summary>
		/// Exchange password for connecting to email account used in EmailFetcher
		/// </summary>
		[ConfigurationProperty("ExchangeEmailPassword")]
		public string ExchangeEmailPassword => (string)this["ExchangeEmailPassword"];

		/// <summary>
		/// Exchange Username for connecting to email account used in EmailFetcher
		/// </summary>
		[ConfigurationProperty("ExchangeEmailUserName")]
		public string ExchangeEmailUserName => (string)this["ExchangeEmailUserName"];

		/// <summary>
		/// Exchange Uri for exchange webservice used in EmailFetcher
		/// </summary>
		[ConfigurationProperty("ExchangeUri", DefaultValue = "https://outlook.office365.com/EWS/Exchange.asmx")]
		public string ExchangeUri => (string)this["ExchangeUri"];

		/// <summary>
		/// Arguments to pass to FFmpeg executable (except file name)
		/// </summary>
		[ConfigurationProperty("FFmpegArguments", DefaultValue = "-f gdigrab -framerate 30 -i desktop", IsRequired = false)]
		public string FFmpegArguments => (string)this["FFmpegArguments"];

		/// <summary>
		/// Timeout for files to be downloaded, value in seconds
		/// </summary>
		[ConfigurationProperty("FileDownloadTimeout", DefaultValue = 60, IsRequired = false)]
		public int FileDownloadTimeout => (int)this["FileDownloadTimeout"];

		/// <summary>
		/// Timeout value for webDriver fluent wait, value in seconds
		/// </summary>
		[ConfigurationProperty("FluentWaitTimeout", DefaultValue = 10, IsRequired = false)]
		public int FluentWaitTimeout => (int)this["FluentWaitTimeout"];

		/// <summary>
		/// Timeout for implicit wait, value in seconds
		/// </summary>
		[ConfigurationProperty("ImplicitWaitTimeout", DefaultValue = 5, IsRequired = false)]
		public int ImplicitWaitTimeout => (int)this["ImplicitWaitTimeout"];

		/// <summary>
		/// Timeout for loading indicator, value in seconds
		/// </summary>
		[ConfigurationProperty("LoadingIndicatorTimeout", DefaultValue = 120, IsRequired = false)]
		public int LoadingIndicatorTimeout => (int)this["LoadingIndicatorTimeout"];

		/// <summary>
		/// Timeout for page load, value in seconds
		/// </summary>
		[ConfigurationProperty("PageLoadTimeout", DefaultValue = 60, IsRequired = false)]
		public int PageLoadTimeout => (int)this["PageLoadTimeout"];

		/// <summary>
		/// Polling timeout used in fluent wait methods, value in milliseconds
		/// </summary>
		[ConfigurationProperty("PollingTimeout", DefaultValue = 50, IsRequired = false)]
		public int PollingTimeout => (int)this["PollingTimeout"];

		/// <summary>
		/// Timeout for document.readyState, value in seconds
		/// </summary>
		[ConfigurationProperty("ReadyStateTimeout", DefaultValue = 120, IsRequired = false)]
		public int ReadyStateTimeout => (int)this["ReadyStateTimeout"];

		/// <summary>
		/// Directory to capture the screenshots
		/// </summary>
		[ConfigurationProperty("ScreenshotsDirectory", DefaultValue = "TestScreenshots", IsRequired = false)]
		public string ScreenshotsDirectory => (string)this["ScreenshotsDirectory"];

		/// <summary>
		/// Sleep timeout used in wait methods, value in milliseconds
		/// </summary>
		[ConfigurationProperty("SleepTimeout", DefaultValue = 250, IsRequired = false)]
		public int SleepTimeout => (int)this["SleepTimeout"];

		/// <summary>
		/// Capture page HTML source on test failure or not
		/// </summary>
		[ConfigurationProperty("TakePageHtmlOnFailure", IsRequired = false)]
		public bool TakePageHtmlOnFailure => (bool)this["TakePageHtmlOnFailure"];

		/// <summary>
		/// Take browser screenshots on test failure or not
		/// </summary>
		[ConfigurationProperty("TakeScreenShotsOnTestFailure", IsRequired = false)]
		public bool TakeScreenShotsOnTestFailure => (bool)this["TakeScreenShotsOnTestFailure"];

		/// <summary>
		/// Take videos of test execution or not
		/// </summary>
		[ConfigurationProperty("TakeVideosOfTestExecution", IsRequired = false)]
		public bool TakeVideosOfTestExecution => (bool)this["TakeVideosOfTestExecution"];

		/// <summary>
		/// Timeout for URLs to be opened, value in seconds
		/// </summary>
		[ConfigurationProperty("UrlOpenTimeout", DefaultValue = 20, IsRequired = false)]
		public int UrlOpenTimeout => (int)this["UrlOpenTimeout"];

		/// <summary>
		/// Directory to capture the videos
		/// </summary>
		[ConfigurationProperty("VideosDirectory", DefaultValue = "Video", IsRequired = false)]
		public string VideosDirectory => (string)this["VideosDirectory"];

		/// <summary>
		/// Retention duration of recorded videos, in days
		/// </summary>
		[ConfigurationProperty("VideosRetentionDuration", DefaultValue = 1, IsRequired = false)]
		public int VideosRetentionDuration => (int)this["VideosRetentionDuration"];

		/// <summary>
		/// Runtime browser type
		/// </summary>
		[ConfigurationProperty("Browser", DefaultValue = "Chrome", IsRequired = true)]
		private string BrowserFromAppConfig => (string)this["Browser"];

		private FrameworkSettings Framework { get; set; }

		/// <inheritdoc />
		public ConfigurationSection Initialize()
		{
			try
			{
				Framework = ConfigurationManager.GetSection("AutomationFrameworkSettings") as FrameworkSettings;
				Log.Debug("Loaded Automation framework settings:" + Environment.NewLine + $"{Framework}");
				return Framework;
			}
			catch (ConfigurationErrorsException e)
			{
				var message = "Failed to load Automation framework settings from app.config. Mandatory settings are missing:";
				Log.Error(message);
				Log.Error(e.Message, e);
				throw new ConfigurationErrorsException($"{message}{Environment.NewLine}{e.Message}");
			}
		}

		/// <inheritdoc />
		public override string ToString() =>
			$"Take screenshots on test failure: {TakeScreenShotsOnTestFailure}" + Environment.NewLine
			                                                                    + $"Directory to capture the screenshots: '{ScreenshotsDirectory}'"
			                                                                    + Environment.NewLine
			                                                                    + $"Capture page HTML on failure: {TakePageHtmlOnFailure}"
			                                                                    + Environment.NewLine + Environment.NewLine
			                                                                    + $"Sleep timeout value, ms: {SleepTimeout}"
			                                                                    + Environment.NewLine
			                                                                    + $"Runtime browser: {BrowserFromAppConfig}"
			                                                                    + Environment.NewLine + $"Exchange Uri: {ExchangeUri} ";
	}
}