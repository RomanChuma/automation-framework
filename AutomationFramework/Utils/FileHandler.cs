using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;

using AutomationFramework.Core.Enums;
using AutomationFramework.Core.Extensions;
using AutomationFramework.Core.Utils.Log;

using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

using Path = System.IO.Path;

namespace AutomationFramework.Core.Utils
{
	public static class FileHandler
	{
		private static readonly ILogger Log = Log4NetLogger.Instance;

		/// <summary>
		/// Current project location
		/// </summary>
		public static string ProjectPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

		/// <summary>
		/// Copy file from one location to another
		/// </summary>
		/// <param name="sourceFilePath">Source file path</param>
		/// <param name="destinationFilePath">Destination file path</param>
		/// <param name="overwriteIfExists">Overwrite if file exists</param>
		public static void CopyFile(string sourceFilePath, string destinationFilePath, bool overwriteIfExists = true)
		{
			if(string.IsNullOrEmpty(sourceFilePath))
			{
				throw new ArgumentNullException(nameof(sourceFilePath));
			}

			if(string.IsNullOrEmpty(destinationFilePath))
			{
				throw new ArgumentNullException(nameof(destinationFilePath));
			}

			try
			{
				var sourceFile = new FileInfo(sourceFilePath);
				string folderPath = GetFolderPathByFileLocation(destinationFilePath);
				CreateDirectoryIfNotExists(folderPath);
				sourceFile.CopyTo(destinationFilePath, overwriteIfExists);
			}
			catch (IOException copyError)
			{
				Log.Warn(copyError.Message, copyError);
				throw;
			}
			catch (Exception ex)
			{
				Log.Error($"Error occurred during copying file - [{sourceFilePath}]", ex);
				throw;
			}
		}

		/// <summary>
		/// Create file
		/// </summary>
		/// <param name="filePath">File path</param>
		/// <param name="fileSize">File size, in bytes</param>
		public static void CreateFile(string filePath, int fileSize = 100)
		{
			if(string.IsNullOrEmpty(filePath))
			{
				throw new ArgumentNullException(nameof(filePath));
			}

			try
			{
				using(FileStream stream = File.Create(filePath))
				{
					stream.Close();
				}

				File.WriteAllBytes(filePath, new byte[fileSize]);
			}
			catch (Exception ex)
			{
				Log.Error($"Error occured during creating file- [{filePath}]", ex);
				throw;
			}
		}

		/// <summary>
		/// Create file
		/// </summary>
		/// <param name="filePath">File path</param>
		/// <param name="fileContent">File content as string, currently use UTF8 encoding </param>
		public static void CreateFile(string filePath, string fileContent)
		{
			if(string.IsNullOrEmpty(filePath))
			{
				throw new ArgumentNullException(nameof(filePath));
			}

			try
			{
				using(FileStream stream = File.Create(filePath))
				{
					stream.Close();
				}

				File.WriteAllBytes(filePath, Encoding.UTF8.GetBytes(fileContent));
			}
			catch (Exception ex)
			{
				Log.Error($"Error occured during creating file- [{filePath}]", ex);
				throw;
			}
		}

		/// <summary>
		/// Create a directory
		/// </summary>
		/// <param name="directoryPath">Directory path</param>
		/// <param name="folderName">Folder name, current date if not specified</param>
		public static string CreateFolder(string directoryPath, string folderName = null)
		{
			if(string.IsNullOrEmpty(folderName))
			{
				folderName = DateUtils.GetCurrentTime("MM-dd-yyyy");
			}

			string combinedPath = Path.Combine(directoryPath, folderName);
			CreateDirectoryIfNotExists(combinedPath);

			return combinedPath;
		}

		/// <summary>
		/// Create folder by relative path
		/// </summary>
		/// <param name="folderPath">Folder path</param>
		/// <param name="isPathRelative">Is path relative</param>
		/// <returns></returns>
		public static string CreateFolder(string folderPath, bool isPathRelative)
		{
			var targetDirectory = string.Empty;
			try
			{
				if(!isPathRelative)
				{
					if(!Directory.Exists(folderPath))
					{
						targetDirectory = Directory.CreateDirectory(folderPath).FullName;
					}
				}
				else
				{
					string currentDirectory = Directory.GetCurrentDirectory();
					targetDirectory = Path.Combine(currentDirectory, folderPath);

					if(!Directory.Exists(targetDirectory))
					{
						targetDirectory = Directory.CreateDirectory(folderPath).FullName;
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error($"Error occured during creating folder by path - [{folderPath}]", ex);
				throw;
			}

			return targetDirectory;
		}

		public static void DeleteFile(string filePath)
		{
			if(File.Exists(filePath))
			{
				File.Delete(filePath);
			}
		}

		public static void DeleteFiles(IEnumerable<string> files)
		{
			foreach(string filePath in files)
			{
				DeleteFile(filePath);
			}
		}

		public static void DeleteFiles(IEnumerable<FileInfo> files)
		{
			foreach(FileInfo file in files)
			{
				file.Delete();
			}
		}

		/// <summary>
		/// Delete files of given pattern from folder
		/// </summary>
		/// <param name="directoryPath">Directory path</param>
		/// <param name="fileType">File search type</param>
		/// <param name="areDeleteFilesReadOnly">Are files read only</param>
		public static void DeleteFilesFromFolder(
			string directoryPath,
			string filePattern,
			bool areDeleteFilesReadOnly = false)
		{
			try
			{
				var fileList = GetFilesByPattern(directoryPath, filePattern);

				foreach(FileInfo fileInfo in fileList)
				{
					if(fileInfo.Exists)
					{
						if(areDeleteFilesReadOnly)
						{
							fileInfo.Attributes = FileAttributes.Normal;
						}

						fileInfo.Delete();
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error($"Exception detected while deleting {filePattern} files from directory {directoryPath}", ex);
				throw;
			}
		}

		/// <summary>
		/// Delete files of given patterns from folder
		/// </summary>
		/// <param name="directoryPath">Directory path</param>
		/// <param name="fileTypes">List of patterns</param>
		/// <param name="areDeleteFilesReadOnly">Are files read only</param>
		public static void DeleteFilesFromFolder(
			string directoryPath,
			List<string> filePatterns,
			bool areDeleteFilesReadOnly = false)
		{
			var fileLists = filePatterns.Select(pattern => GetFilesByPattern(directoryPath, pattern)).ToList();

			try
			{
				foreach(var fileList in fileLists)
				{
					foreach(FileInfo fileInfo in fileList)
					{
						if(fileInfo.Exists)
						{
							if(areDeleteFilesReadOnly)
							{
								fileInfo.Attributes = FileAttributes.Normal;
							}

							fileInfo.Delete();
						}
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error($"Exception detected while deleting files from directory {directoryPath}", ex);
				throw;
			}
		}

		/// <summary>
		/// Delete files of given pattern from folder
		/// </summary>
		/// <param name="directoryPath">Directory path</param>
		/// <param name="fileType">File search type</param>
		/// <param name="areDeleteFilesReadOnly">Are files read only</param>
		public static void DeleteFilesFromFolderByType(
			string directoryPath,
			FileType fileType,
			bool areDeleteFilesReadOnly = false)
		{
			try
			{
				var fileList = GetFilesByType(directoryPath, fileType);

				foreach(FileInfo fileInfo in fileList)
				{
					if(fileInfo.Exists)
					{
						if(areDeleteFilesReadOnly)
						{
							fileInfo.Attributes = FileAttributes.Normal;
						}

						fileInfo.Delete();
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error(
					$"Exception detected while deleting {fileType.GetDescription()} files from directory {directoryPath}",
					ex);
				throw;
			}
		}

		/// <summary>
		/// Delete files of given patterns from folder
		/// </summary>
		/// <param name="directoryPath">Directory path</param>
		/// <param name="fileTypes">List of patterns</param>
		/// <param name="areDeleteFilesReadOnly">Are files read only</param>
		public static void DeleteFilesFromFolderByType(
			string directoryPath,
			List<FileType> fileTypes,
			bool areDeleteFilesReadOnly = false)
		{
			var fileLists = fileTypes.Select(pattern => GetFilesByType(directoryPath, pattern)).ToList();

			try
			{
				foreach(var fileList in fileLists)
				{
					foreach(FileInfo fileInfo in fileList)
					{
						if(fileInfo.Exists)
						{
							if(areDeleteFilesReadOnly)
							{
								fileInfo.Attributes = FileAttributes.Normal;
							}

							fileInfo.Delete();
						}
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error($"Exception detected while deleting files from directory {directoryPath}", ex);
				throw;
			}
		}

		/// <summary>
		/// Delete files older than specified retention policy
		/// </summary>
		/// <param name="directoryPath">Directory path</param>
		/// <param name="retentionDuration">Retention policy</param>
		public static void DeleteFilesOlderThan(string directoryPath, TimeSpan retentionDuration)
		{
			var filesOlderThanSpecifiedPolicy = GetFilesOlderThan(directoryPath, retentionDuration);
			DeleteFiles(filesOlderThanSpecifiedPolicy);
		}

		public static void ExtractZipFileToDirectory(
			string folderPath,
			string zipFileName,
			string destinationFolderPath = "Default")
		{
			if(destinationFolderPath == "Default")
			{
				destinationFolderPath = folderPath;
			}

			string zipFilePath = $"{folderPath}\\{zipFileName}";
			ZipFile.ExtractToDirectory(zipFilePath, destinationFolderPath);
			Log.Trace($"{zipFileName} was extracted to {destinationFolderPath}");
		}

		/// <summary>
		/// Verify if specific file is present in the folder. Can verify exact file name, or file pattern (ex: *.pdf)
		/// </summary>
		/// <param name="folderPath">Path to the specific folder</param>
		/// <param name="fileType">File name, or pattern to get the filename (ex: "*.pdf" for any pdf file in folder)</param>
		/// <returns></returns>
		public static bool FileIsPresentInFolder(string folderPath, FileType fileType)
		{
			var fileNames = GetFilesByType(folderPath, fileType);
			return fileNames.Length > 0;
		}

		public static bool FileIsPresentInFolder(string folderPath, string filePattern)
		{
			var fileNames = GetFilesByPattern(folderPath, filePattern);
			return fileNames.Length > 0;
		}

		/// <summary>
		/// Gets the list of file names form the folder
		/// </summary>
		/// <param name="sourceDirectoryPath">Directory path</param>
		/// <param name="filePattern">File pattern</param>
		/// <returns>the list of file names</returns>
		public static List<string> GetFileNamesByPattern(string sourceDirectoryPath, string filePattern)
		{
			var fileList = GetFilesByPattern(sourceDirectoryPath, filePattern);
			var fileNames = fileList.Select(file => file.Name).ToList();
			return fileNames;
		}

		/// <summary>
		/// Gets the list of names, of the specific file types
		/// </summary>
		/// <param name="sourceDirectoryPath">Directory path</param>
		/// <param name="fileType">File type</param>
		/// <returns>the list of file names</returns>
		public static List<string> GetFileNamesByType(string sourceDirectoryPath, FileType fileType)
		{
			var fileList = GetFilesByType(sourceDirectoryPath, fileType);
			var fileNames = fileList.Select(file => file.Name).ToList();
			return fileNames;
		}

		/// <summary>
		/// Get folder files by given file pattern
		/// </summary>
		/// <param name="sourceDirectoryPath">Directory path</param>
		/// <param name="filePattern">File pattern</param>
		/// <returns></returns>
		public static FileInfo[] GetFilesByPattern(string sourceDirectoryPath, string filePattern= "*")
		{
			FileInfo[] fileList;

			try
			{
				var sourceDirectory = new DirectoryInfo(sourceDirectoryPath);

				if(!Directory.Exists(sourceDirectory.FullName))
				{
					throw new IOException($"Directory '{sourceDirectory.FullName}' does not exist!");
				}

				fileList = sourceDirectory.GetFiles(filePattern);
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message, ex);
				throw;
			}

			return fileList;
		}

		/// <summary>
		/// Gets array of the files with specific type
		/// </summary>
		/// <param name="sourceDirectoryPath"></param>
		/// <param name="fileType"></param>
		/// <returns></returns>
		public static FileInfo[] GetFilesByType(string sourceDirectoryPath, FileType fileType)
		{
			string filePattern = $"*.{fileType.GetDescription()}";
			var fileList = GetFilesByPattern(sourceDirectoryPath, filePattern.ToLower());
			return fileList;
		}

		/// <summary>
		/// Get file size in bytes
		/// </summary>
		/// <param name="folderPath">Folder path</param>
		/// <param name="fileName">File name</param>
		/// <returns></returns>
		public static double GetFileSize(string folderPath, string fileName)
		{
			long fileSize = new FileInfo(GetTargetFileDirectory(folderPath, fileName)).Length;
			return fileSize;
		}

		/// <summary>
		/// Gets file size by it's location using units (kilobytes, megabytes etc.)
		/// </summary>
		/// <param name="fileLength">Length of file in bytes</param>
		/// <returns>String</returns>
		public static string GetFileSizeWithUnits(long fileLength)
		{
			const long Kilobytes = 1024;
			const long Megabytes = Kilobytes * 1024;
			double size = fileLength;
			var suffix = "Bytes";

			bool isFileSizeInMegabytes = fileLength >= Megabytes;
			bool isFileSizeInKilobytes = fileLength >= Kilobytes;

			if(isFileSizeInMegabytes)
			{
				size = Math.Round((double)fileLength / Megabytes, digits: 2);
				suffix = "Mb";
			}
			else if(isFileSizeInKilobytes)
			{
				size = Math.Round((double)fileLength / Kilobytes, digits: 2);
				suffix = "Kb";
			}

			return $"{size} {suffix}";
		}

		/// <summary>
		/// Get files older than specified age
		/// </summary>
		/// <param name="directoryPath">Directory path</param>
		/// <param name="fileAge">File creation age</param>
		/// <returns>List of <see cref="FileInfo"/> of files older than current date minus age</returns>
		public static List<FileInfo> GetFilesOlderThan(string directoryPath, TimeSpan fileAge)
		{
			var fileList = GetFilesByPattern(directoryPath, "*");
			var filesOlderThanSpecifiedPolicy = new List<FileInfo>();
			DateTime borderDate = DateTime.UtcNow - fileAge;

			foreach(FileInfo file in fileList)
			{
				var fileIsOlderThanSpecifiedPolicy = DateTime.Compare(file.CreationTimeUtc, borderDate) < 0;

				if(fileIsOlderThanSpecifiedPolicy)
				{
					filesOlderThanSpecifiedPolicy.Add(file);
				}
			}

			return filesOlderThanSpecifiedPolicy;
		}

		/// <summary>
		/// Get folder path to given file
		/// </summary>
		/// <param name="filePath">Full file path</param>
		/// <returns></returns>
		public static string GetFolderPathByFileLocation(string filePath)
		{
			var sourceFile = new FileInfo(filePath);
			return sourceFile.DirectoryName;
		}

		/// <summary>
		/// Get readable file size representation (Currently method supports bytes and kilobytes. Should be extended by need)
		/// </summary>
		/// <param name="fileSize">File size</param>
		public static string GetMemoryUnitsFileSize(long fileSize) =>
			fileSize.ToString("0.0## ") + GetSuffixByFileSize(fileSize);

		/// <summary>
		/// Get relative folder path
		/// </summary>
		/// <param name="folderPath">Folder path</param>
		/// <returns>string</returns>
		public static string GetRelativeFolderPath(string folderPath)
		{
			string targetDirectory;
			try
			{
				string currentDirectory = Directory.GetCurrentDirectory();
				targetDirectory = Path.Combine(currentDirectory, folderPath);
			}
			catch (Exception ex)
			{
				Log.Error("Error occured during folder operation!", ex);
				throw;
			}

			return targetDirectory;
		}

		/// <summary>
		/// Get a relative path from one file or folder to another.
		/// </summary>
		/// <param name="startPath">Contains the directory that defines the start of the relative path.</param>
		/// <param name="toPath">Contains the path that defines the endpoint of the relative path.</param>
		/// <returns>The relative path from the start directory to the end path or <c>toPath</c> if the paths are not related.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="UriFormatException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		public static string GetRelativePath(string startPath, string toPath)
		{
			if(string.IsNullOrEmpty(startPath))
			{
				throw new ArgumentNullException(nameof(startPath));
			}

			if(string.IsNullOrEmpty(toPath))
			{
				throw new ArgumentNullException(nameof(toPath));
			}

			var fromUri = new Uri(startPath);
			var toUri = new Uri(toPath);

			if(fromUri.Scheme != toUri.Scheme)
			{
				return toPath;
			}

			// path can't be made relative.
			Uri relativeUri = fromUri.MakeRelativeUri(toUri);
			string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

			if(toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
			{
				relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
			}

			return relativePath;
		}

		/// <summary>
		/// Get file target directory
		/// </summary>
		/// <param name="folderPath">Folder path</param>
		///  <param name="fileName">File name</param>
		/// <returns>string</returns>
		public static string GetTargetFileDirectory(string folderPath, string fileName)
		{
			string targetDirectory;
			try
			{
				string currentBinDirectory = Directory.GetCurrentDirectory();
				string projectDirectory = Path.Combine(currentBinDirectory, @"..\..\");
				string projectDirectoryFullPath = Path.GetFullPath(projectDirectory);
				targetDirectory = Path.Combine(projectDirectoryFullPath, folderPath, fileName);
			}
			catch (Exception ex)
			{
				Log.Error("Error occured during folder operation!", ex);
				throw;
			}

			return targetDirectory;
		}

		/// <summary>
		/// Read file content
		/// </summary>
		/// <param name="filePath">File path</param>
		/// <returns>string</returns>
		public static string ReadFileContent(string filePath)
		{
			string streamContent;

			if(File.Exists(filePath))
			{
				FileStream fileStream = null;

				try
				{
					fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
					using(var streamReader = new StreamReader(fileStream, Encoding.UTF8))
					{
						streamContent = streamReader.ReadToEnd();
						fileStream = null;
					}
				}
				catch (Exception ex)
				{
					Log.Error($"Opening the file '{filePath}' caused exception.", ex);
					throw;
				}
				finally
				{
					fileStream?.Dispose();
				}
			}
			else
			{
				throw new FileNotFoundException($"Cannot read content from file '{filePath}' because it is missing!");
			}

			return streamContent;
		}

		/// <summary>
		/// Parses PDF file to sting
		/// </summary>
		/// <param name="filePath">Full path to the desired file (ex: "C:\Downloads\SomeFile.pdf" )</param>
		/// <returns>String content from PDF</returns>
		public static string ReadPdfFileToString(string filePath)
		{
			var reader = new PdfReader(filePath);

			var stringBuilder = new StringBuilder();

			for(var page = 1; page <= reader.NumberOfPages; page++)
			{
				string content = PdfTextExtractor.GetTextFromPage(reader, page);
				stringBuilder.Append(content);
			}

			reader.Close();
			var text = stringBuilder.ToString();

			return text;
		}

		private static void CreateDirectoryIfNotExists(string path)
		{
			if(!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}

		/// <summary>
		/// Determine the suffix and readable value
		/// </summary>
		private static string GetSuffixByFileSize(long fileSize)
		{
			const int Hexadecimal = 0x400;
			string suffix;
			if(fileSize >= Hexadecimal)
			{
				suffix = "KB";
			}
			else
			{
				if(fileSize == 0)
				{
					suffix = "byte";
				}
				else
				{
					suffix = "bytes";
				}
			}

			return suffix;
		}
	}
}