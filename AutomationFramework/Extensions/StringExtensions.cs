using System;
using System.Linq;

namespace AutomationFramework.Core.Extensions
{
	/// <summary>
	/// Class for string extension methods
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Convert first character of string to uppercase
		/// </summary>
		/// <param name="input">Input string</param>
		/// <returns>String with first capital character and rest of the characters converted to lowercase</returns>
		public static string ToUppercaseFirstChar(this string input)
		{
			switch(input)
			{
				case null:
					throw new ArgumentNullException(nameof(input));
				case "":
					throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
				default:
					return input.First().ToString().ToUpper() + input.Substring(1).ToLower();
			}
		}

		/// <summary>
		/// Get a substring of the first N characters.
		/// </summary>
		/// <param name="inputString">Input string</param>
		/// <param name="maxLength">Maximum length</param>
		/// <returns>Truncated string</returns>
		public static string Truncate(this string inputString, int maxLength)
		{
			if(inputString.Length > maxLength)
			{
				inputString = inputString.Substring(0, maxLength);
			}

			return inputString;
		}

		/// <summary>
		/// Remove whitespace within a string
		/// </summary>
		/// <param name="input">Input string</param>
		/// <returns>String without whitespaces</returns>
		public static string RemoveWhitespace(this string input) => new string(input.ToCharArray()
		                                                                            .Where(c => !char.IsWhiteSpace(c))
		                                                                            .ToArray());

		/// <summary>
		/// Remove trailing slash (right and left) from the string
		/// </summary>
		/// <param name="input">Input string</param>
		/// <returns>String without trailing slash</returns>
		public static string RemoveTrailingSlash(this string input)
		{
			char[] slashes = { '/', '\\' };
			return input.TrimEnd(slashes);
		}
	}
}