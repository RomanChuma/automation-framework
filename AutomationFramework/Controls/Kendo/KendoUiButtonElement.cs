using AutomationFramework.Core.Controls.Interfaces;

using OpenQA.Selenium;

namespace AutomationFramework.Core.Controls.Kendo
{
	/// <summary>
	/// Represents the Kendo UI Button element
	/// See https://docs.telerik.com/kendo-ui/controls/navigation/button/overview
	/// </summary>
	public class KendoUiButtonElement : UiElement, IButton
	{
		private readonly IWebElement _kendoButton;

		public KendoUiButtonElement(IWebElement webElement)
			: base(webElement)
		{
			_kendoButton = webElement;
		}

		public bool IsEnabled
		{
			get
			{
				// See https://docs.telerik.com/kendo-ui/controls/navigation/button/disabled for details
				string classAttribute = _kendoButton.GetAttribute("class").ToLowerInvariant();
				bool isDisabled = classAttribute.Contains("disabled");
				return !isDisabled;
			}
		}

		public string Text => _kendoButton.Text;
	}
}