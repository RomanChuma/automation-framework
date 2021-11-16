using AutomationFramework.Core.Controls.Interfaces;

using OpenQA.Selenium;

namespace AutomationFramework.Core.Controls.Grid
{
	/// <summary>
	/// Header of the grid element
	/// </summary>
	public class GridTableHeaderElement : UiElement, IGridTableHeader
	{
		private readonly IWebElement _webElement;

		public GridTableHeaderElement(IWebElement webElement)
			: base(webElement)
		{
			_webElement = webElement;
		}

		/// <summary>
		/// Grid columns
		/// </summary>
		public GridColumns Columns => new GridColumns(_webElement);
	}
}