using System.Collections.Generic;

using AutomationFramework.Core.Controls.Kendo.Grid;

namespace AutomationFramework.Core.Controls.Interfaces
{
	public interface IKendoGrid<T> : IGrid<T>
		where T : UiElement, IGridRow
	{
		KendoGridHeader Header { get; }

		List<KendoGridColumn> GetColumns();
	}
}