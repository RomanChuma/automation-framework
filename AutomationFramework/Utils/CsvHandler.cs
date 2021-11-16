using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using AutomationFramework.Core.Utils.Log;

using CsvHelper;

namespace AutomationFramework.Core.Utils
{
    public static class CsvHandler
    {
        private static readonly ILogger Log = Log4NetLogger.Instance;

        /// <summary>
        /// Write records to CSV File to provided filepath
        /// </summary>
        /// <typeparam name="T">
        /// Type
        /// </typeparam>
        /// <param name="fullFilePath">
        /// Full file path of csv to write
        /// </param>
        /// <param name="recordsToWrite">
        /// Records to write
        /// </param>
        public static void WriteToCsvFile<T>(string fullFilePath, IEnumerable<T> recordsToWrite)
        {
            try
            {
                TextWriter textWriter = new StreamWriter(fullFilePath);

                // CSV writer stream
                var csvWriter = new CsvWriter(textWriter);

                // Write all records
                csvWriter.WriteRecords(recordsToWrite);

                // Close StreamWriter
                textWriter.Close();
            }
            catch (Exception e)
            {
                Log.Warn($"Exception in writing {recordsToWrite} to csv at {fullFilePath}");
                Log.Error(e.Message, e);
            }
        }

        /// <summary>
        /// Reads records from CSV File
        /// </summary>
        /// <typeparam name="T">
        /// Type
        /// </typeparam>
        /// <param name="fullFilePath">
        /// Full file path of csv to read
        /// </param>
        /// <returns>
        /// Collection of records
        /// </returns>
        public static IEnumerable<T> ReadFromCsvFile<T>(string fullFilePath)
        {
            IEnumerable<T> records = null;

            try
            {
                // Read File content
                string fileContent = FileHandler.ReadFileContent(fullFilePath);
                TextReader textReader = new StringReader(fileContent);
                var csvReader = new CsvReader(textReader);

                // As csv may consists of huge number of headers and may also have missing fields, we have to set it to null to avoid exceptions
                csvReader.Configuration.HeaderValidated = null;
                csvReader.Configuration.MissingFieldFound = null;

                // Read csv as records
                records = csvReader.GetRecords<T>();
            }
            catch (Exception e)
            {
                Log.Warn($"Exception in reading from csv at {fullFilePath}");
                Log.Error(e.Message, e);
            }

            return records;
        }

        /// <summary>
        /// Count records from csv
        /// </summary>
        /// <param name="folderPath">Folder path for csv file</param>
        /// <param name="fileName">Name of csv file</param>
        /// <param name="containsHeader">Csv contains header row</param>
        /// <param name="delimiter">Delimiter used in file</param>
        /// <returns>int</returns>
        public static int GetCountOfRecordsFromCsv(string folderPath, string fileName, bool containsHeader = true, string delimiter = "\r")
        {
            int recordCount = 0;
            string filePath = $"{folderPath}\\{fileName}";

            try
            {
                string content = File.ReadAllText(filePath);
                recordCount = Regex.Matches(content, delimiter).Count;

                if (containsHeader)
                {
                    recordCount -= 1;
                }

            }
            catch (Exception e)
            {
                Log.Warn($"Exception in getting count of records from csv : '{fileName}'");
                Log.Error(e.Message, e);
            }

            return recordCount;
        }

    }
}
