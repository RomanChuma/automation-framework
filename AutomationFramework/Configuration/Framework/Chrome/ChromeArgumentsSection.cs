using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace AutomationFramework.Core.Configuration.Framework.Chrome
{
	internal class ChromeArgumentsSection : IConfigurationSectionHandler
	{
		public object Create(object parent, object configContext, XmlNode section)
		{
			var chromeArguments = new List<string>();

			foreach (XmlNode childNode in section.ChildNodes)
			{
				foreach (XmlAttribute attribute in childNode.Attributes)
				{
					chromeArguments.Add(attribute.Value);
				}
			}

			return chromeArguments;
		}
	}
}
