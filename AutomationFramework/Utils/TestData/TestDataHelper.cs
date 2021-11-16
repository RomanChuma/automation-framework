using System;
using System.IO;

using AutomationFramework.Core.Utils.Log;

namespace AutomationFramework.Core.Utils.TestData
{
    public static class TestDataHelper
    {
        private static readonly ILogger Log = Log4NetLogger.Instance;

        /// <summary>
        /// Get test data files directory
        /// </summary>
        /// <param name="testFilesFolderPath">Test data folder path</param>
        /// <returns>string</returns>
        public static string GetTestDataDirectory(string testFilesFolderPath)
        {
            string testFilesTargetDirectory;
            try
            {
                string currentBinDirectory = Directory.GetCurrentDirectory();
                string testProjectDirectory = Path.Combine(currentBinDirectory, @"..\..\");
                string testProjectDirectoryFullPath = Path.GetFullPath(testProjectDirectory);
                testFilesTargetDirectory = Path.Combine(testProjectDirectoryFullPath, testFilesFolderPath);
            }
            catch (Exception ex)
            {
                Log.Error("Error occured during folder operation!", ex);
                throw;
            }

            return testFilesTargetDirectory;
        }
    }
}
