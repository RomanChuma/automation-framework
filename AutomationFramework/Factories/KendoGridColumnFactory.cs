using System.Collections.Generic;

using AutomationFramework.Core.Controls.Kendo.Grid;

namespace AutomationFramework.Core.Factories
{
	internal static class KendoGridColumnFactory
	{
		public static KendoGridColumn Create(Dictionary<string, object> columnConfig)
		{
			var kendoColumn = new KendoGridColumn { Title = SetColumnTitle(columnConfig) };
			return kendoColumn;
		}

		private static string SetColumnTitle(Dictionary<string, object> configValue)
		{
			var title = string.Empty;

			if (configValue.ContainsKey("title"))
			{
				title = configValue["title"].ToString();
			}

			return title;
		}
	}
}