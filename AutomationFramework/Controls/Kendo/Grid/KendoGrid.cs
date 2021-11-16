using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using AutomationFramework.Core.Controls.Grid;
using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Engine;
using AutomationFramework.Core.Factories;
using AutomationFramework.Core.Utils;

using OpenQA.Selenium;

namespace AutomationFramework.Core.Controls.Kendo.Grid
{
	/// <summary>
	/// Represents the Kendo UI Grid widget
	/// </summary>
	/// <typeparam name="T">Grid row type</typeparam>
	public class KendoGrid<T> : GridElement<T>, IKendoGrid<T>
		where T : KendoGridRow
	{
		/// <summary>
		/// Wrapper around grid
		/// </summary>
		protected readonly IWebElement GridWrapper;

		public KendoGrid(IWebElement webElement)
			: base(webElement)
		{
			GridWrapper = webElement;
		}

		public KendoGridHeader Header => new KendoGridHeader(TableHeader);

		/// <summary>
		/// Gets the jQuery object which represents the grid table header element
		/// </summary>
		private IWebElement TableHeader
		{
			get
			{
				try
				{
					var header = (IWebElement)Browser.InvokeScript("return $('.k-grid').getKendoGrid().thead");
					return header;
				}
				catch (Exception e)
				{
					Log.Error("Was not able to get table header element", e);
					throw;
				}
			}
		}

		/// <summary>
		/// Get the configuration of the grid columns
		/// </summary>
		/// <returns>List of <see cref="KendoGridColumn"/></returns>
		public List<KendoGridColumn> GetColumns()
		{
			var rawColumnsConfig = GetColumnsConfiguration();
			var kendoColumns = new List<KendoGridColumn>();

			foreach (var configValue in rawColumnsConfig)
			{
				kendoColumns.Add(KendoGridColumnFactory.Create(configValue));
			}

			return kendoColumns;
		}

		public override List<T> GetRows()
		{
			var webDriverRows = GetRowsAsIWebElements().ToList();
			var kendoRows = new List<T>();

			foreach (IWebElement foundRow in webDriverRows)
			{
				KendoGridRow kendoRow = KendoGridRowFactory.Create(foundRow, webDriverRows.IndexOf(foundRow));
				var castedRow = (T)kendoRow;
				kendoRows.Add(castedRow);
			}

			return kendoRows;
		}

		public void WaitToBeLoaded()
		{
			var waitScript = "return document.getElementsByClassName('k-loading-mask').length === 0";

			try
			{
				TimeSpan timeOutForLoadingMaskToDisappear = TimeSpan.FromSeconds(30);
				Wait.Until(() => Convert.ToBoolean(Browser.InvokeScript(waitScript)), timeOutForLoadingMaskToDisappear);

				TimeSpan timeoutForHtmlToRebuild = TimeSpan.FromMilliseconds(250);
				Wait.For(timeoutForHtmlToRebuild);
			}
			catch (TimeoutException)
			{
				Log.Error("Loading mask did not disappear on the grid");
				throw;
			}
		}

		protected IEnumerable<IWebElement> GetRowsAsIWebElements()
		{
			try
			{
				object result = Browser.InvokeScript("return $('.k-grid').getKendoGrid().tbody[0].rows;");
				var webDriverRows = (ReadOnlyCollection<IWebElement>)result;
				return webDriverRows;
			}
			catch (Exception e)
			{
				Log.Error("Was not able to get Kendo UI grid rows", e);
				throw;
			}
		}

		/// <summary>
		/// Get raw JSON array of grid column configuration
		/// See https://docs.telerik.com/kendo-ui/api/javascript/ui/grid/configuration/columns
		/// </summary>
		/// <returns>Grid columns configuration</returns>
		private List<Dictionary<string, object>> GetColumnsConfiguration()
		{
			var columnConfiguration = new List<Dictionary<string, object>>();
			try
			{
				var columnsAsJsonArray = (ReadOnlyCollection<object>)Browser.InvokeScript("return $('.k-grid').getKendoGrid().columns");

				foreach (object configArray in columnsAsJsonArray)
				{
					var config = configArray as Dictionary<string, object>;

					columnConfiguration.Add(config);
				}
			}
			catch (Exception e)
			{
				Log.Error("Was not able to obtain Kendo UI columns configuration", e);
				throw;
			}

			return columnConfiguration;
		}
	}
}