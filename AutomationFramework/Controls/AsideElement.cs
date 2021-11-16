using AutomationFramework.Core.Controls.Interfaces;

using OpenQA.Selenium;

namespace AutomationFramework.Core.Controls
{
	public class AsideElement : UiElement, IAside
	{
		public AsideElement(IWebElement webElement)
			: base(webElement)
		{
		}
	}
}