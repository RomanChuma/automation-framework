using AutomationFramework.Core.Enums;

namespace AutomationFramework.Core.Controls.Interfaces
{
	public interface IGridCell : IHtmlElement, IClickable, ITextContent
	{
		GridSortOrder SortOrder { get; }
	}
}
