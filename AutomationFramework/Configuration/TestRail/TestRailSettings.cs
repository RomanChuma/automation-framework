using System;
using System.Configuration;

using AutomationFramework.Core.Utils.Log;

namespace AutomationFramework.Core.Configuration.TestRail
{
	public class TestRailSettings : ConfigurationSection, IConfiguration
	{
		private static readonly ILogger Log = Log4NetLogger.Instance;

		/// <summary>
		/// TestRail URL
		/// </summary>
		[ConfigurationProperty("URL", IsRequired = false)]
		public string Url
		{
			get
			{
				var url = (string)this["URL"];

				if (url == string.Empty)
				{
					Log.Error("Test Rail url value can not be empty. Verify your app.config");
				}

				return url;
			}
		}

		/// <summary>
		/// TestRail username
		/// </summary>
		[ConfigurationProperty("Username", IsRequired = false)]
		public string Username
		{
			get
			{
				var username = (string)this["Username"];

				if (username == string.Empty)
				{
					Log.Error("Test Rail username value can not be empty. Verify your app.config");
				}

				return username;
			}
		}

		/// <summary>
		/// TestRail password
		/// </summary>
		[ConfigurationProperty("Password", IsRequired = false)]
		public string Password
		{
			get
			{
				var password = (string)this["Password"];

				if (password == string.Empty)
				{
					Log.Error("Test Rail password value can not be empty. Verify your app.config");
				}

				return password;
			}
		}

		/// <summary>
		/// TestRail test run ID
		/// </summary>
		[ConfigurationProperty("TestRunId", IsRequired = false)]
		public int TestRunId
		{
			get
			{
				var testRunId = (int)this["TestRunId"];
				return testRunId;
			}
		}

		public override string ToString() => $"TestRail URL: {Url}" + Environment.NewLine + $"Test run ID: '{TestRunId}'";

		/// <inheritdoc />
		public ConfigurationSection Initialize()
		{
			try
			{
				var testRail = ConfigurationManager.GetSection("TestRailSettings") as TestRailSettings;
				Log.Debug("Loaded TestRail settings:" +
						 Environment.NewLine +
						 $"{testRail}");
				return testRail;
			}
			catch (ConfigurationErrorsException e)
			{
				var message = "Failed to load TestRail settings from app.config. Mandatory settings are missing:";
				Log.Error(message);
				Log.Error(e.Message, e);
				throw new ConfigurationErrorsException($"{message}" +
													   Environment.NewLine +
													   $"{e.Message}");
			}
		}
	}
}