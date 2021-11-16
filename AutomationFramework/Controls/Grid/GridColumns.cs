using System;
using System.Collections.Generic;
using System.Linq;

using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Utils.Log;

using OpenQA.Selenium;

using By = AutomationFramework.Core.Engine.By;

namespace AutomationFramework.Core.Controls.Grid
{
	/// <summary>
	/// Grid columns element implementation
	/// </summary>
	public class GridColumns : UiElement
	{
		private readonly List<GridCellElement> _columns;

		private readonly ILogger _log = Log4NetLogger.Instance;

		public GridColumns(IWebElement webElement)
			: base(webElement)
		{
			_columns = FindElements<GridCellElement>(By.XPath("//th")).ToList();
		}

		/// <summary>
		/// Gets column count
		/// </summary>
		public int Count => _columns.Count;

		/// <summary>
		/// Gets first grid column
		/// </summary>
		public IGridCell FirstColumn => _columns[0];

		/// <summary>
		/// Gets last grid column
		/// </summary>
		public IGridCell LastColumn => _columns[_columns.IndexOf(_columns.Last())];

		public IGridCell this[int columnIndex] => _columns[columnIndex];

		public IGridCell this[string columnText]
		{
			get
			{
				GridCellElement column = _columns.FirstOrDefault(x => x.Text.Equals(columnText));

				if (column == null)
				{
					string message = $"Column with name '{columnText}' is not found in grid." + Environment.NewLine
					                                                                          + "Make sure that column name has correct capitalization, does not contain whitespaces or special characters";
					_log.Error(message);
					throw new NotFoundException(message);
				}

				return column;
			}
		}

		/// <summary>
		/// All of the titles of columns, including empty strings
		/// </summary>
		public List<string> GetPreservedTitles() => _columns.Select(x => x.Text).ToList();

		/// <summary>
		/// Column titles (without empty headers)
		/// </summary>
		public List<string> GetTitles() => _columns.Select(x => x.Text).Where(x => x != string.Empty).ToList();
	}
}