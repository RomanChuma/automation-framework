using OpenQA.Selenium;

namespace AutomationFramework.Core.Controls.Kendo.Grid
{
	/// <summary>
	/// Kendo UI grid table header element
	/// </summary>
	public class KendoGridHeader : UiElement
	{
		internal KendoGridHeader(IWebElement webElement)
			: base(webElement)
		{
		}
	}
}