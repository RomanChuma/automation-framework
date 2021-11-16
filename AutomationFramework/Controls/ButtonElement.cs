using AutomationFramework.Core.Controls.Interfaces;

using OpenQA.Selenium;

namespace AutomationFramework.Core.Controls
{
	/// <summary>
	/// Concrete implementation of button element
	/// </summary>
	public class ButtonElement : UiElement, IButton
	{
		private readonly IWebElement _button;

		public ButtonElement(IWebElement webElement) : base(webElement)
		{
			_button = webElement;
		}

		public string Text => _button.Text;

		/// <summary>
		/// Is element enabled
		/// </summary>
		public bool IsEnabled => _button.Enabled;
	}
}