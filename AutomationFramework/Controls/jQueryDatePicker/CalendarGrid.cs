using AutomationFramework.Core.Controls.Grid;
using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Engine;

using OpenQA.Selenium;

using By = AutomationFramework.Core.Engine.By;

namespace AutomationFramework.Core.Controls.jQueryDatePicker
{
	public class CalendarGrid : GridElement<GridRowElement>
	{
		/// <inheritdoc />
		public CalendarGrid(IWebElement webElement)
			: base(webElement)
		{
		}

		public IGridTableHeader Header =>
			Browser.FindElement<GridTableHeaderElement>(By.XPath("//table[@class='ui-datepicker-calendar']/thead"));
	}
}