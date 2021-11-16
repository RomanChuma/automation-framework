using AutomationFramework.Core.Controls.Interfaces;

using OpenQA.Selenium;

namespace AutomationFramework.Core.Controls
{
	public class DivElement : UiElement, IDiv
	{
		private readonly IWebElement _webElement;

		public DivElement(IWebElement webElement) : base(webElement)
		{
			_webElement = webElement;
		}

		/// <inheritdoc />
		public string Text => _webElement.Text;

		public string Value => _webElement.GetAttribute("value");
	}
}