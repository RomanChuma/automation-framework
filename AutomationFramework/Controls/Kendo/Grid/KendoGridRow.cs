using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using AutomationFramework.Core.Controls.Grid;
using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Engine;

using OpenQA.Selenium;

namespace AutomationFramework.Core.Controls.Kendo.Grid
{
	/// <summary>
	/// Row implementation for <see cref="KendoGrid{T}"/>
	/// </summary>
	public class KendoGridRow : UiElement, IGridRow
	{
		internal readonly int Index;

		private readonly IWebElement _kendoRow;

		public KendoGridRow(IWebElement webElement, int index)
			: base(webElement)
		{
			_kendoRow = webElement;
			Index = index;
		}

		public string Text => _kendoRow.Text;

		public string Value => _kendoRow.GetAttribute("value");

		public List<GridCellElement> GetCells()
		{
			try
			{
				var webDriverCells =
					(ReadOnlyCollection<IWebElement>)Browser.InvokeScript($"return $('.k-grid').getKendoGrid().tbody[0].rows[{Index}].cells");

				var gridCells = new List<GridCellElement>();

				foreach (IWebElement foundCell in webDriverCells)
				{
					var gridCell = new GridCellElement(foundCell);
					gridCells.Add(gridCell);
				}

				return gridCells;
			}
			catch (Exception e)
			{
				Log.Error($"Was not able to get cells for Kendo UI grid row with index '{Index}'", e);
				throw;
			}
		}
	}
}