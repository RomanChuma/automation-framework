using System;
using System.Collections.Generic;
using System.Linq;

using Bogus;

namespace AutomationFramework.Core.Utils.RandomUtils
{
	public class Lorem
	{
		internal Lorem()
		{
		}

		/// <summary>
		/// Random lorem word
		/// </summary>
		public string Word => new Faker().Lorem.Word();

		/// <summary>
		/// Get lorem words
		/// </summary>
		/// <param name="numberOfWords">Number of words</param>
		/// <returns>List of words</returns>
		public List<string> GetWords(int numberOfWords = 3) => new Faker().Lorem.Words(numberOfWords).ToList();

		/// <summary>
		/// Generate random string using GUID of given length
		/// </summary>
		/// <param name="length">String length, 8 by default</param>
		/// <returns>Random string using GUID of given length</returns>
		public string GetRandomString(int length = 8)
		{
			string randomString = Guid.NewGuid().ToString().Substring(0, length);
			return randomString;
		}

		/// <summary>
		/// Get random alphanumeric sequence of given length
		/// </summary>
		/// <param name="length">Length of the sequence</param>
		/// <returns>Random alphanumeric sequence</returns>
		public string GetRandomAlphaNumeric(int length = 5)
		{
			var characters =
				"ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
				"abcdefghijklmnopqrstuvwxyz";
			var numbers = "0123456789";

			string alphanumericCharacters = characters + numbers;

			var random = new Random();
			return new string(Enumerable.Repeat(alphanumericCharacters, length)
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}
	}
}
