using AutomationFramework.Core.Controls.Interfaces;

using OpenQA.Selenium;

namespace AutomationFramework.Core.Controls
{
	public class MainElement : UiElement, IMain
	{
		public MainElement(IWebElement webElement) : base(webElement)
		{
		}
	}
}
