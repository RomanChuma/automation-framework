using System.Collections.Generic;

using AutomationFramework.Core.Controls.Interfaces;

namespace AutomationFramework.Core.Controls.Grid.Pager
{
	/// <summary>
	/// Contract for page numbering present in grids.
	/// </summary>
	public interface IPageNumbering
	{
		/// <summary>
		/// Gets active button.
		/// </summary>
		IButton BtnActivePage { get; }

		List<ButtonElement> GetPageButtons();

		/// <summary>
		/// Is page indicator of given page number active
		/// </summary>
		/// <param name="pageNumber">
		/// The number of page.
		/// </param>
		/// <returns>
		/// Boolean is page selected
		/// </returns>
		bool IsPageSelected(int pageNumber);

		/// <summary>
		/// Select last page in the grid footer
		/// </summary>
		void SelectLastPage();

		void SelectPage(int pageNumber);
	}
}