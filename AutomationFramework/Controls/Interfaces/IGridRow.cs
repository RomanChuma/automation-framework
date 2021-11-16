using System.Collections.Generic;

using AutomationFramework.Core.Controls.Grid;

namespace AutomationFramework.Core.Controls.Interfaces
{
	public interface IGridRow : IHtmlElement, IClickable, ITextContent
	{
		List<GridCellElement> GetCells();
	}
}
