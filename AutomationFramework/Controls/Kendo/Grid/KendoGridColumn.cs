namespace AutomationFramework.Core.Controls.Kendo.Grid
{
	/// <summary>
	/// Column implementation for <see cref="KendoGrid{T}"/>
	/// See https://docs.telerik.com/kendo-ui/api/javascript/ui/grid/fields/columns
	/// </summary>
	public class KendoGridColumn
	{
		internal KendoGridColumn()
		{
		}

		/// <summary>
		/// Gets value of 'title' attribute
		/// </summary>
		public string Title { get; internal set; }
	}
}
