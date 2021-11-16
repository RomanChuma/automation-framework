using System;
using System.Linq;

using AutomationFramework.Core.Engine;
using AutomationFramework.Core.Enums;

using OpenQA.Selenium;

using By = AutomationFramework.Core.Engine.By;

namespace AutomationFramework.Core.Controls.jQueryDatePicker
{
	/// <summary>
	/// Concrete implementation of custom jQuery datepicker element
	/// See https://api.jqueryui.com/datepicker/
	/// </summary>
	public class JQueryDatePickerElement : UiElement
	{
		/// <inheritdoc />
		public JQueryDatePickerElement(IWebElement webElement)
			: base(webElement)
		{
		}

		public ButtonPane ButtonPane =>
			Browser.FindElement<ButtonPane>(By.XPath("//div[@class='ui-datepicker-buttonpane ui-widget-content']"));

		public CalendarGrid GrdCalendar =>
			Browser.FindElement<CalendarGrid>(By.XPath("//table[@class='ui-datepicker-calendar']"));

		public DatePickerHeader Header =>
			Browser.FindElement<DatePickerHeader>(
				By.XPath("//div[@class='ui-datepicker-header ui-widget-header ui-helper-clearfix ui-corner-all']"));

		/// <summary>
		/// Select given date from the JQuery datepicker element
		/// </summary>
		/// <param name="dateToBeSelected">Date to be Selected in Format (yyyy-MM-dd)</param>
		public void SelectDate(string dateToBeSelected)
		{
			string year = dateToBeSelected.Split('-').ElementAt(0);
			int month = int.Parse(dateToBeSelected.Split('-').ElementAt(1));
			int day = int.Parse(dateToBeSelected.Split('-').ElementAt(2));

			string monthName = ((JQueryDatePickerMonth)month).ToString();

			if (year != null)
			{
				SelectYear(year);
			}

			if (monthName != JQueryDatePickerMonth.NotSet.ToString())
			{
				SelectMonth(monthName);
			}

			SelectDay(day);
		}

		/// <summary>
		/// Select day on calendar grid
		/// </summary>
		/// <param name="dayNumber">Day number (date)</param>
		public void SelectDay(int dayNumber)
		{
			if (dayNumber < 1 && dayNumber > 31)
			{
				throw new ArgumentOutOfRangeException(nameof(dayNumber));
			}

			int rowIndex = GrdCalendar.GetRowIndexByCellValue(dayNumber.ToString());
			var dayLinksFromRow = GrdCalendar.GetRows()[rowIndex].GetUnderlyingElements<AnchorElement>();
			AnchorElement targetDayLink =
				dayLinksFromRow.FirstOrDefault(date => date.Text.Equals(dayNumber.ToString()));
			targetDayLink.Click();
		}

		/// <summary>
		/// Select month from JQuery datepicker header
		/// </summary>
		/// <param name="month">Month to select</param>
		public void SelectMonth(string month)
		{
			Header.DdlMonth.SelectOptionByText(month);
		}

		/// <summary>
		/// Select year from JQuery datepicker header
		/// </summary>
		/// <param name="year">Year to select</param>
		public void SelectYear(string year)
		{
			Header.DdlYear.SelectOptionByText(year);
		}
	}
}