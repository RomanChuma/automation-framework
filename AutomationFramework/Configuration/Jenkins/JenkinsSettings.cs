using System;
using System.Configuration;

using AutomationFramework.Core.Utils.Log;

namespace AutomationFramework.Core.Configuration.Jenkins
{
	public class JenkinsSettings : ConfigurationSection, IConfiguration
	{
		private static readonly ILogger Log = Log4NetLogger.Instance;
		

		/// <summary>
		/// Jenkins URL (without http:// since we need to add user:ApiToken before)
		/// </summary>
		[ConfigurationProperty("JenkinsUrl", IsRequired = false)]
		public string JenkinsUrl
		{
			get
			{
				var url = (string)this["JenkinsUrl"];

				if (url == string.Empty)
				{
					Log.Error("Jenkins url value can not be empty. Verify your app.config");
				}

				return url;
			}
		}

		/// <summary>
		/// Jenkins username
		/// </summary>
		[ConfigurationProperty("Username", IsRequired = false)]
		public string Username
		{
			get
			{
				var username = (string)this["Username"];

				if (username == string.Empty)
				{
					Log.Error("Jenkins username value can not be empty. Verify your app.config");
				}

				return username;
			}
		}

		/// <summary>
		/// Jenkins ApiToken
		/// </summary>
		[ConfigurationProperty("ApiToken", IsRequired = false)]
		public string ApiToken
		{
			get
			{
				var apiToken = (string)this["ApiToken"];

				if (apiToken == string.Empty)
				{
					Log.Error("Jenkins ApiToken value can not be empty. Verify your app.config");
				}

				return apiToken;
			}
		}

		public override string ToString() => $"Jenkins URL: {JenkinsUrl}, Jenkins ApiToken: {ApiToken}";

		public ConfigurationSection Initialize()
		{
			try
			{
				var jenkins = ConfigurationManager.GetSection("JenkinsSettings") as JenkinsSettings;
				Log.Debug("Loaded Jenkins settings:" +
				          Environment.NewLine +
				          $"{jenkins}");
				return jenkins;
			}
			catch (ConfigurationErrorsException e)
			{
				var message = "Failed to load jenkins settings from app.config. Mandatory settings are missing:";
				Log.Error(message);
				Log.Error(e.Message, e);
				throw new ConfigurationErrorsException($"{message}" +
				                                       Environment.NewLine +
				                                       $"{e.Message}");
			}
		}
	}
}