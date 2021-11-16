using AutomationFramework.Core.Controls.Grid;

namespace AutomationFramework.Core.Controls.Interfaces
{
	public interface IGridTableHeader : IHtmlElement
	{
		GridColumns Columns { get; }
	}
}
