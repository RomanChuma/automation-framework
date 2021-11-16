using AutomationFramework.Core.Controls.Interfaces;

using OpenQA.Selenium;

namespace AutomationFramework.Core.Controls
{
	public class FooterElement : UiElement, IFooter
	{
		public FooterElement(IWebElement webElement) : base(webElement)
		{
		}
	}
}