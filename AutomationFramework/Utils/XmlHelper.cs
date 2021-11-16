using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

using AutomationFramework.Core.Utils.Log;

namespace AutomationFramework.Core.Utils
{
	public static class XmlHelper
	{
		private static readonly ILogger Log = Log4NetLogger.Instance;


		/// <summary>
		/// Load XElement from file.
		/// </summary>
		/// <param name="path">Path to the folder</param>
		/// <param name="fileName">Name of the file that contains XML</param>
		/// <returns>X Element</returns>
		public static XElement LoadXElementFromFile(string path, string fileName)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentException("XML path can't be null or empty");
			}

			if (string.IsNullOrEmpty(fileName))
			{
				throw new ArgumentException("XML fileName can't be null or empty");
			}

			bool pathEndsWithoutBackslash = !path.Last().Equals('\\');

			if (pathEndsWithoutBackslash)
			{
				path = path + '\\';
			}

			try
			{
				return XElement.Load(path + fileName);
			}
			catch (Exception e)
			{
				Log.Warn($"Exception raised on loading XML: {e.Message}");
				throw;
			}
		}

		/// <summary>
		/// Create a XElement based on xml string
		/// </summary>
		/// <param name="stringXml">Input XMl string</param>
		/// <returns>XElement representation of XML string</returns>
		public static XElement GetXElementFromString(string stringXml)
		{
			if (string.IsNullOrEmpty(stringXml))
			{
				throw new ArgumentException("Input XML string can't be null or empty");
			}

			var document = XElement.Parse(stringXml);

			return document;
		}

		/// <summary>
		/// Validate a XML string against XSD schema file
		/// </summary>
		/// <param name="documentXml">String representation of XML to validate</param>
		/// <param name="xsdFilePath">XSD file path</param>
		/// <returns>True if validation is successful, false otherwise</returns>
		public static bool ValidateXsdSchema(XElement documentXml, string xsdFilePath)
		{
			var document = new XDocument();
			document.Add(documentXml);
			var schemas = new XmlSchemaSet();

			// Add schema  using  default namespace
			schemas.Add(string.Empty, xsdFilePath);

			bool schemaIsValid = true;
			int errorCounter = 0;

			Log.Debug("Beginning validation of XML document against XSD schema");

			var stringBuilder = new StringBuilder();

			// Validation handler is only triggered when error event if fired
			document.Validate(schemas, (x, e) =>
			{
				stringBuilder.Append($"{errorCounter + 1}: Encountered {e.Severity} during validation of XML");
				stringBuilder.AppendLine();
				stringBuilder.Append($"Exception: {e.Exception}");
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();

				// ToString returns actual XML representation of XML object were XSD error occurred.
				stringBuilder.Append(x.ToString());
				stringBuilder.AppendLine();

				Log.Error(stringBuilder.ToString());

				stringBuilder.Clear();
				++errorCounter;
				schemaIsValid = false;
			});

			Log.Debug("Finished validation of XML against XSD schema");

			if (!schemaIsValid)
			{
				throw new XmlSchemaValidationException("Validation of xml against XSD schema failed");
			}

			return schemaIsValid;
		}

		/// <summary>
		/// Verify if string contains valid XML
		/// </summary>
		/// <param name="stringContainingXml">Source string with XML content</param>
		/// <returns>Verification result</returns>
		public static bool VerifyIfStringContainsXml(string stringContainingXml)
		{
			try
			{
				new XmlDocument().Load(new StringReader(stringContainingXml));
				return true;
			}
			catch (Exception e)
			{
				Log.Warn($"Exception raised on verifying XML: {e.Message}");
				return false;
			}
		}
	}
}