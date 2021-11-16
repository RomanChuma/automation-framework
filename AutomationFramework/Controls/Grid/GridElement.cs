using System;
using System.Collections.Generic;
using System.Linq;

using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Enums;
using AutomationFramework.Core.Extensions;
using AutomationFramework.Core.Utils;

using OpenQA.Selenium;

using By = AutomationFramework.Core.Engine.By;

namespace AutomationFramework.Core.Controls.Grid
{
	/// <summary>
	/// Grid element implementation
	/// </summary>
	/// <typeparam name="T">
	/// Row Element
	/// </typeparam>
	public class GridElement<T> : UiElement, IGrid<T>
		where T : UiElement, IGridRow
	{
		private readonly UiElement _table;

		protected GridElement(IWebElement webElement)
			: base(webElement)
		{
			_table = webElement.As<UiElement>();
		}

		/// <summary>
		/// Check multiple rows
		/// </summary>
		/// <param name="multipleRowsIndexList">
		/// Multiple rows index list
		/// </param>
		/// <param name="actionType">
		///  Type of action to be performed
		/// </param>
		public virtual void CheckMultipleRowsByIndex(
			List<int> multipleRowsIndexList,
			ActionType actionType = ActionType.JavaScript)
		{
			var checkBoxColumnIndex = 0;
			try
			{
				foreach (int row in multipleRowsIndexList)
				{
					GetRows()[row].GetCells()[checkBoxColumnIndex].GetUnderlyingElement<InputCheckboxElement>()
					              .Check(actionType);
				}
			}
			catch (InvalidOperationException e)
			{
				Log.Warn("Exception occured while checking multiple rows from the grid" + e);
			}
		}

		/// <summary>
		/// Check tax return document row of given index in a grid
		/// </summary>
		/// <param name="rowIndex">
		/// Zero based row index
		/// </param>
		/// <param name="actionType">
		/// Type of action to be performed
		/// </param>
		public virtual void CheckRowByIndex(int rowIndex, ActionType actionType = ActionType.Default)
		{
			var checkBoxColumnIndex = 0;
			GetRows()[rowIndex].GetCells()[checkBoxColumnIndex].GetUnderlyingElement<InputCheckboxElement>()
			                   .Check(actionType);
		}

		/// <summary>
		/// Get cell inner text in column of given index
		/// </summary>
		/// <param name="columnIndex">Column index</param>
		/// <param name="rowIndex">Row index</param>
		/// <returns>Cell text</returns>
		public string GetCellTextByColumnIndex(int columnIndex, int rowIndex) =>
			GetRows()[rowIndex].GetCells()[columnIndex].Text;

		public List<int> GetIndexesOfRowsContainingText(string textToSearch)
		{
			var rowsContainingTheText = new List<T>();

			var rows = GetRows();

			foreach (T row in rows)
			{
				bool targetTextIsFoundInRow = row.Text.Contains(textToSearch);

				if (targetTextIsFoundInRow)
				{
					rowsContainingTheText.Add(row);
				}
			}

			var rowIndexes = new List<int>();

			foreach (T row in rowsContainingTheText)
			{
				int rowIndex = rows.IndexOf(row);
				rowIndexes.Add(rowIndex);
			}

			return rowIndexes;
		}

		/// <summary>
		/// Find row which contains specific text
		/// </summary>
		/// <param name="textToFind">Text to find in row</param>
		/// <returns>Grid row element</returns>
		public virtual T GetRowByTextMatch(string textToFind)
		{
			var allRows = GetRows();
			T rowFound = allRows.FirstOrDefault(row => row.Text.Contains(textToFind));
			return rowFound;
		}

		/// <summary>
		/// Get row index by given cell value
		/// </summary>
		/// <param name="cellValue">Cell value to search in rows</param>
		/// <returns>Index of found row</returns>
		public virtual int GetRowIndexByCellValue(string cellValue)
		{
			var allRows = GetRows();
			T rowWithTargetCellValue = allRows.First(cells => cells.Text.Contains(cellValue));
			int rowIndex = allRows.IndexOf(rowWithTargetCellValue);

			return rowIndex;
		}

		public virtual List<T> GetRows() => _table.FindElements<T>(By.XPath(".//tbody//tr")).ToList();

		/// <summary>
		/// Check that scrollbar is present on the grid
		/// </summary>
		/// <param name="scrollBarType">Scrollbar type</param>
		/// <returns>Boolean result</returns>
		public bool IsScrollBarPresent(ScrollBarType scrollBarType)
		{
			var isElementScrollBarPresent = false;

			switch (scrollBarType)
			{
				case ScrollBarType.Horizontal:
					isElementScrollBarPresent = Convert.ToBoolean(
						InvokeJavaScript("return arguments[0].scrollWidth < arguments[0].offsetHeight;"));
					break;

				case ScrollBarType.Vertical:
					isElementScrollBarPresent = Convert.ToBoolean(
						InvokeJavaScript("return arguments[0].scrollWidth > arguments[0].offsetHeight;"));
					break;
			}

			return isElementScrollBarPresent;
		}

		/// <summary>
		/// Wait grid to update after the action is performed.
		/// NOTE: The number of rows is not expected to change.
		/// </summary>
		/// <example>Call this method by passing some action causes the table to be rebuilt
		/// <code>
		/// _documentPage.DlgT2EfileAttachments.TabSelect.WaitForGridToReload(
		/// () => _documentPage.DlgT2EfileAttachments.ClickButton(DialogButton.UploadAndSave));
		/// </code>
		/// </example>
		/// <param name="action">The action causes the table to be rebuilt</param>
		public virtual void WaitToReload(Action action)
		{
			try
			{
				var rowsBeforeAction = GetRows();
				action();

				if (rowsBeforeAction.Count > 0)
				{
					try
					{
						Wait.Until(
							() =>
								{
									bool rowsAmountHasChanged = rowsBeforeAction.Count != GetRows().Count;
									Guid updatedFirstRowUuId = GetRows().First().GetUuId();
									Guid initialFirstRowUuid = rowsBeforeAction.First().GetUuId();
									bool firstRowIsUpdated = initialFirstRowUuid != updatedFirstRowUuId;
									return rowsAmountHasChanged || firstRowIsUpdated;
								});
						return;
					}
					catch (TimeoutException e)
					{
						var message = "Amount of rows within the grid did not changed and first row was not refreshed";
						Log.Error(message, e);
					}

					WaitForAnyRowToBeRefreshedWithoutChangingTotalCount(rowsBeforeAction);
				}
			}
			catch (Exception ex)
			{
				var message = "Error was occured while waiting grid to reload after action was performed.";
				Log.Error(message, ex);
				throw new InvalidOperationException(message, ex);
			}
		}

		private void WaitForAnyRowToBeRefreshedWithoutChangingTotalCount(List<T> rowsBeforeAction)
		{
			Wait.Until(
				() =>
					{
						var atLeastOneElementWasUpdated = false;
						bool rowsCountIsSame = GetRows().Count == rowsBeforeAction.Count;

						if (rowsCountIsSame)
						{
							foreach (T row in rowsBeforeAction)
							{
								int rowIndex = rowsBeforeAction.IndexOf(row);
								Guid initialRowId = row.GetUuId();
								Guid updatedRowId = GetRows()[rowIndex].GetUuId();
								bool rowIdChanged = initialRowId != updatedRowId;

								if (rowIdChanged)
								{
									atLeastOneElementWasUpdated = true;
									break;
								}
							}
						}

						bool gridWasUpdated = rowsCountIsSame && atLeastOneElementWasUpdated;
						return gridWasUpdated;
					});
		}
	}
}