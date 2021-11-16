using ExcelDataReader;
using System;
using System.Data;
using System.IO;
using AutomationFramework.Core.Utils.Log;

namespace AutomationFramework.Core.Utils.ExcelHelper
{
	public static class ExcelHelper
	{
		private static readonly ILogger Log = Log4NetLogger.Instance;

		/// <summary>
		/// This method is used to get data table from excel files (.xls or .xlsx)
		/// </summary>
		/// <param name="dataSheetName">Sheet name</param>
		/// /// <param name="filepath">File Path</param>
		/// <returns>Data Table</returns>
		public static DataTable GetDataTableFromExcel(string dataSheetName, string filepath)
		{
			DataTable invoiceDataTable = null;
			Stream stream = null;
			if (string.IsNullOrEmpty(filepath))
			{
				throw new ArgumentNullException(nameof(filepath));
			}
			if (string.IsNullOrEmpty(dataSheetName))
			{
				throw new ArgumentNullException(nameof(dataSheetName));
			}

			try
			{
				IExcelDataReader excelReader;
				stream = File.Open(filepath, FileMode.Open, FileAccess.Read);
				var extension = Path.GetExtension(filepath);
				switch (extension)
				{
					case ".xlsx":
						{
							// new xlsx format
							excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
							break;
						}

					case ".xls":
						{
							//old xls format
							excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
							break;
						}

					default:
						{
							throw new NotSupportedException("Other file types are not supported.");
						}
				}

				DataSet result = excelReader.AsDataSet(new ExcelDataSetConfiguration
				{
					ConfigureDataTable = _ => new ExcelDataTableConfiguration
					{
						UseHeaderRow = true // means first row is column name 

					},
				});

				invoiceDataTable = result.Tables[$"{dataSheetName}"];
				excelReader.Close();
			}

			catch (Exception exception)
			{
				Log.Warn("Exception detected while handling excel file.");
				Log.Error(exception.Message, exception);
				Log.Error(exception.StackTrace, exception);
			}
			finally
			{
				if (stream != null)
				{
					stream.Close();
				}
			}
			return invoiceDataTable;
		}
	}
}
