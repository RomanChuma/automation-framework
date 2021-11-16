using System.Text.RegularExpressions;

namespace AutomationFramework.Core.Extensions
{
    /// <summary>
    /// String Extension methods for regular expressions usage
    /// </summary>
    public static class RegexConvertUtils
    {
        /// <summary>
        /// Remove text characters from the string
        /// </summary>
        /// <param name="input">String value</param>
        /// <returns>Numeric value</returns>
        /// <example>Before conversion:  '2016 and later'; After conversion: 2016</example>
        public static string ConvertToNumericOnly(this string input)
        {
            var regexMask = new Regex("[^0-9]");
			string replacement = string.Empty;
			return regexMask.Replace(input, replacement);
        }

        /// <summary>
        /// Get converted to alpha numeric only string
        /// </summary>
        /// <param name="input">string value</param>
        /// <returns>string</returns>
        public static string ToAlphaNumericOnly(this string input)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9]");
            return rgx.Replace(input, string.Empty);
        }

        /// <summary>
        /// Get converted to alpha only string
        /// </summary>
        /// <param name="input">string value</param>
        /// <returns>string</returns>
        /// <example>Before conversion:  numeric + alpha; After conversion: Removes numeric</example>
        public static string ToAlphaOnly(this string input)
        {
            Regex rgx = new Regex("[^a-zA-Z]");
            return rgx.Replace(input, string.Empty);
        }
    }
}
