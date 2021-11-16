using System.Collections.Generic;
using System.Configuration;

using OpenQA.Selenium.Chrome;

namespace AutomationFramework.Core.Engine
{
	public partial class Browser
	{
		private static ChromeOptions ChromeProfile
		{
			get
			{
				var chromeOptions = new ChromeOptions();

				// Read arguments from App.config
				if (ConfigurationManager.GetSection("ChromeArguments") is List<string> arguments)
				{
					chromeOptions.AddArguments(arguments);

                    // todo move this option to App.config
				    chromeOptions.AddUserProfilePreference("plugins.always_open_pdf_externally", true);
					chromeOptions.AddUserProfilePreference("safebrowsing.enabled", true);
				}

				return chromeOptions;
			}
		}
	}
}