using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AutomationFramework.Core.Utils.RandomUtils
{
	public class Numbers
	{
		internal Numbers()
		{
		}

		/// <summary>
		/// Get random numbers
		/// </summary>
		/// <param name="length">Length</param>
		/// <returns>string</returns>
		public long GetRandomNumbers(int length)
		{
			var random = new Random();
			var numbers = "0123456789";
			var result = new string(Enumerable.Repeat(numbers, length)
			   .Select(s => s[random.Next(s.Length)]).ToArray());

			// Convert string to long
			long.TryParse(result, out long randomNumber);
			return randomNumber;
		}

		/// <summary>
		/// Get random int number from interval in the given boundaries
		/// </summary>
		/// <param name="min">Minimum value</param>
		/// <param name="max">Maximum value</param>
		/// <returns>Random integer</returns>
		public int GetRandomNumberFromInterval(int min, int max)
		{
			var seed = Convert.ToInt32(Regex.Match(Guid.NewGuid().ToString(), @"\d+").Value);
			var result = new Random(seed).Next(min, max);
			return result;
		}
	}
}
