using AutomationFramework.Core.Controls.Kendo.Grid;

using OpenQA.Selenium;

namespace AutomationFramework.Core.Factories
{
	internal static class KendoGridRowFactory
	{
		internal static KendoGridRow Create(IWebElement webElement, int index)
		{
			var row = new KendoGridRow(webElement, index);
			return row;
		}
	}
}