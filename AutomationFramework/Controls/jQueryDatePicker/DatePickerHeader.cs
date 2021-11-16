using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Engine;

using OpenQA.Selenium;

using By = AutomationFramework.Core.Engine.By;

namespace AutomationFramework.Core.Controls.jQueryDatePicker
{
	/// <summary>
	/// Concrete implementation of header for jQuery datepicker
	/// </summary>
	public class DatePickerHeader : DivElement
	{
		/// <inheritdoc />
		public DatePickerHeader(IWebElement webElement)
			: base(webElement)
		{
		}

		public IAnchor LnkPrevious =>
			Browser.FindElement<AnchorElement>(
				By.XPath("//a[@class='ui-datepicker-prev ui-corner-all' and @data-handler='prev']"));

		public IAnchor LnkNext =>
			Browser.FindElement<AnchorElement>(
				By.XPath("//a[@class='ui-datepicker-next ui-corner-all' and @data-handler='next']"));

		public IDropDown DdlMonth =>
			Browser.FindElement<DropdownElement>(By.XPath("//select[@class='ui-datepicker-month']"));

		public IDropDown DdlYear =>
			Browser.FindElement<DropdownElement>(By.XPath("//select[@class='ui-datepicker-year']"));
	}
}
