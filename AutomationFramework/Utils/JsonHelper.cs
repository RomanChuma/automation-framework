using Newtonsoft.Json.Linq;

namespace AutomationFramework.Core.Utils
{
	/// <summary>
	/// Helper class for work with JSON
	/// </summary>
	public static class JsonHelper
	{
		/// <summary>
		/// Validates string if it is a valid JSON object
		/// </summary>
		/// <param name="inputJson">Input string</param>
		/// <returns>Boolean validation result</returns>
		public static bool IsValidJson(string inputJson)
		{
			inputJson = inputJson.Trim();

			bool inputIsJsonObject = inputJson.StartsWith("{") && inputJson.EndsWith("}");
			bool inputIsJsonArray = inputJson.StartsWith("[") && inputJson.EndsWith("]");

			if (inputIsJsonObject || inputIsJsonArray)
			{
				try
				{
					JToken.Parse(inputJson);
					return true;
				}
				catch
				{
					return false;
				}
			}

			return false;
		}
	}
}