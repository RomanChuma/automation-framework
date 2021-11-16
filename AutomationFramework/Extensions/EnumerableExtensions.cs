using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomationFramework.Core.Extensions
{
	/// <summary>
	/// Extension methods for types that implement <see cref="IEnumerable{T}"/>
	/// </summary>
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Get single random value from the collection
		/// </summary>
		/// <typeparam name="T">Collection type</typeparam>
		/// <param name="source">Collection, to take value from</param>
		/// <returns>Single item of type T"/></returns>
		public static T PickRandom<T>(this IEnumerable<T> source) => source.PickRandom(1).Single();

		/// <summary>
		/// Get range of random values from the collection
		/// </summary>
		/// <typeparam name="T">Collection type</typeparam>
		/// <param name="source">Collection, to take value from</param>
		/// <param name="count">Count of elements in range</param>
		/// <returns>Range of random items of type T</returns>
		public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count) => source.Shuffle().Take(count);

		/// <summary>
		/// Randomize values in collection
		/// </summary>
		/// <typeparam name="T">Collection type</typeparam>
		/// <param name="source">Collection, to randomize</param>
		/// <returns>Randomized collection</returns>
		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source) => source.OrderBy(x => Guid.NewGuid());

		/// <summary>
		/// Converts IEnumerable of strings into integers
		/// </summary>
		/// <param name="collectionOfStrings">Input collection</param>
		/// <returns>Converted collection of integers</returns>
		public static IEnumerable<int> ConvertStringsToIntegers(this IEnumerable<string> collectionOfStrings)
		{
			var convertedValues = new List<int>();

			foreach (string stringValue in collectionOfStrings)
			{
				var intValue = Convert.ToInt32(stringValue);
				convertedValues.Add(intValue);
			}

			return convertedValues;
		}

		/// <summary>
		/// Converts IEnumerable of integers into strings
		/// </summary>
		/// <param name="collectionOfIntegers">Input collection</param>
		/// <returns>Converted collection of strings</returns>
		public static IEnumerable<string> ConvertIntegersToStrings(this IEnumerable<int> collectionOfIntegers)
		{
			var convertedValues = new List<string>();

			foreach (int intValue in collectionOfIntegers)
			{
				var stringValue = Convert.ToString(intValue);
				convertedValues.Add(stringValue);
			}

			return convertedValues;
		}
	}
}
