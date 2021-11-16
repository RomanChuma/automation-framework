using System;

using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Engine;
using AutomationFramework.Core.Enums;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace AutomationFramework.Core.Controls.Grid
{
	/// <summary>
	/// Grid cell element
	/// </summary>
	public class GridCellElement : UiElement, IGridCell
	{
		private readonly IWebElement _gridCell;

		public GridCellElement(IWebElement webElement)
			: base(webElement)
		{
			_gridCell = webElement;
		}

		public GridSortOrder SortOrder
		{
			get
			{
				string actualClass = GetUnderlyingElement<SpanElement>().GetAttribute("class");
				var ascendingClassValue = "sort-icon fa fa-caret-up";

				if (actualClass.Contains(ascendingClassValue))
				{
					return GridSortOrder.Ascending;
				}

				return GridSortOrder.Descending;
			}
		}

		public string Text => _gridCell.Text.Trim();

		public string Title => _gridCell.GetAttribute("title");

		public string Value => _gridCell.GetAttribute("value");

		public void WaitForTextToBePresent(string elementText, int timeOut = 20)
		{
			var wait = new WebDriverWait(Browser.Instance, TimeSpan.FromSeconds(timeOut));
			Log.Trace($"Waiting for {OuterHtml} element to contain text '{elementText}' on '{Browser.Title}' page");
			wait.Until(ExpectedConditions.TextToBePresentInElement(_gridCell, elementText));
			Log.Trace($"'{elementText}' text appeared");
		}
	}
}