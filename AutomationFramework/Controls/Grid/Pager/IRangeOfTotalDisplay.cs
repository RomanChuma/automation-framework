namespace AutomationFramework.Core.Controls.Grid.Pager
{
	/// <summary>
	/// Contract for grid that displays items range of total
	/// </summary>
	public interface IRangeOfTotalDisplay
	{
		/// <summary>
		/// Gets the number of the first item displayed on this page
		/// </summary>
		/// <returns>
		/// <see cref="int"/> number of the first item.
		/// </returns>
		int NumberOfFirstItemDisplayed { get; }

		/// <summary>
		/// Gets the number of the last item displayed on this page
		/// </summary>
		/// <returns>
		/// <see cref="int"/> number of the last item.
		/// </returns>
		int NumberOfLastItemDisplayed { get; }

		/// <summary>
		/// Gets the number of total items
		/// </summary>
		int NumberOfTotalItems { get; }
	}
}