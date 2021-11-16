using AutomationFramework.Core.Controls.Interfaces;

namespace AutomationFramework.Core.Controls.Grid.Pager
{
	/// <summary>
	/// Contract for grid that has per page selection dropdown
	/// </summary>
	public interface IPerPageSelector
	{
		/// <summary>
		/// Per page selector
		/// </summary>
		IDropDown PerPageSelector { get; }
	}
}