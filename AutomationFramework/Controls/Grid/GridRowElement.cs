using System.Collections.Generic;
using System.Linq;

using AutomationFramework.Core.Controls.Interfaces;

using OpenQA.Selenium;

using By = AutomationFramework.Core.Engine.By;

namespace AutomationFramework.Core.Controls.Grid
{
	public class GridRowElement : UiElement, IGridRow
	{
		private readonly IWebElement _row;

		public GridRowElement(IWebElement webElement)
			: base(webElement)
		{
			_row = webElement;
		}

		public string Text => _row.Text.Trim();

		public string Value => _row.GetAttribute("value");

		public List<GridCellElement> GetCells() => FindElements<GridCellElement>(By.XPath(".//td")).ToList();
	}
}