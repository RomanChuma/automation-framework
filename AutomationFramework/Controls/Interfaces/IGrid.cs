using System.Collections.Generic;

using AutomationFramework.Core.Enums;

namespace AutomationFramework.Core.Controls.Interfaces
{
	public interface IGrid<T> : IHtmlElement, IClickable
		where T : IHtmlElement, IGridRow
	{
		void CheckMultipleRowsByIndex(List<int> multipleRowsIndexList, ActionType actionType);

		void CheckRowByIndex(int rowIndex, ActionType actionType);

		string GetCellTextByColumnIndex(int columnIndex, int rowIndex);

		T GetRowByTextMatch(string textToFind);

		List<T> GetRows();

		bool IsScrollBarPresent(ScrollBarType scrollBarType);
	}
}