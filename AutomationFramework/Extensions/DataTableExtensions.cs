using AutomationFramework.Core.Enums;
using AutomationFramework.Core.Utils.Log;
using System;
using System.Data;

namespace AutomationFramework.Core.Extensions
{
	public static class DataTableExtensions
	{
		private static readonly ILogger Log = Log4NetLogger.Instance;

		/// <summary>
		/// Performs operation such as sum or count on column of data table
		/// </summary>
		/// <param name="dataTable">Data table</param>
		/// <param name="column">Column name</param>
		/// <param name="operation">The operation to be performed</param>
		/// <returns>Returns object type</returns>
		public static object PerformOperationOnDataTable(this DataTable dataTable, string column, ExcelOperation operation)
		{
			object result = null;
			try
			{
				switch (operation)
				{
					case ExcelOperation.Sum:
						result = dataTable.Compute($"Sum([{column}])", string.Empty);
						break;
					case ExcelOperation.Count:
						result = dataTable.Compute($"Count([{column}])", string.Empty);
						break;
				}
			}
			catch (Exception exception)
			{
				Log.Warn("Exception detected in extension method for handling excel file.");
				Log.Error(exception.Message, exception);
				Log.Error(exception.StackTrace, exception);
			}

			return result;
		}
	}
}