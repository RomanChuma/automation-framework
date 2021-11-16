using System;
using System.Collections.Generic;
using System.Globalization;

using Bogus;

namespace AutomationFramework.Core.Utils.RandomUtils
{
	public class Person
	{
		internal Person()
		{
		}

		public string FirstName => new Bogus.Person().FirstName;

		public string LastName => new Bogus.Person().LastName;

		public string FullName => new Bogus.Person().FullName;

		public DateTime DateOfBirth => new Bogus.Person().DateOfBirth;

		public string Title => new Faker().Name.Prefix();

		public string Phone => new Faker().Phone.PhoneNumber("##########");

		public string Email => new Bogus.Person().Email;

		public string JobTitle => new Faker().Name.JobTitle();

		public string WebSite => new Bogus.Person().Website;

		public string Skype => new Bogus.Person().Email;

		public string Twitter => "@" + RandomData.Lorem.GetRandomAlphaNumeric(99);

		public string LinkedIn => "https://www.linkedin.com/in/" +
								  RandomData.Lorem.GetRandomAlphaNumeric(20) +
								  "-" +
								  RandomData.Lorem.GetRandomAlphaNumeric(40) +
								  "-" +
								  RandomData.Lorem.GetRandomAlphaNumeric(10);

		/// <summary>
		/// Get Social Insurance number
		/// </summary>
		/// <returns></returns>
		public string GetSin()
		{
			// Wait is added because creating several contacts causes in a row, causes generation of the same SIN. Wait will give us another timestamp for SIN generation
			Wait.For(TimeSpan.FromMilliseconds(1));

			int sinLength = 8;
			var sin = GetInitialSinValue(sinLength);
			bool sinIsInvalidOrIsImmigrantsOrTemporary = VerifyThatFirstCharacterOfSinIsValid(sin);

			if (sinIsInvalidOrIsImmigrantsOrTemporary)
			{
				sin = GetValidSin(sin);
			}

			string controlNumber = CalculateControlNumber(sin).ToString();
			sin += controlNumber;
			return sin;
		}

		private static string GetValidSin(string sin)
		{
			var random = new Random();
			int firstRandomValue = random.Next(1, 9);
			sin = firstRandomValue + sin.Substring(1);

			return sin;
		}

		private bool VerifyThatFirstCharacterOfSinIsValid(string sin)
		{
			var firstSinCharacter = sin.Substring(0, 1);
			string firstCharacterForImmigrantsAndTempSins = "9";
			string firstCharacterForInvalidSins = "0";
			bool sinIsInvalid = firstSinCharacter == firstCharacterForInvalidSins;
			bool sinIsImmigrantsOrTemporary = firstSinCharacter == firstCharacterForImmigrantsAndTempSins;
			bool sinIsInvalidOrIsImmigrantsOrTemporary = sinIsInvalid || sinIsImmigrantsOrTemporary;

			return sinIsInvalidOrIsImmigrantsOrTemporary;
		}

		private string GetInitialSinValue(int sinLength)
		{
			var millisecondsInStringFormat = GetTimeInMillicesonds();
			var sin = millisecondsInStringFormat.Substring(millisecondsInStringFormat.Length - sinLength);
			return sin;
		}

		private string GetTimeInMillicesonds()
		{
			var timeInMilliseconds = DateUtils.GenerateDateTimeInMilliseconds();
			var stringRepresentation = Convert.ToString(Math.Floor(timeInMilliseconds), CultureInfo.CurrentCulture);
			return stringRepresentation;
		}

		private int CalculateControlNumber(string inputSin)
		{
			// Making the list to easy multiplication digits
			var controlList = new List<int> { 1, 2, 1, 2, 1, 2, 1, 2 };
			var sinNumbers = SplitSinIntoIntegers(inputSin);
			var multiplicationResults = new List<int>();

			// Multiplying each digit in Sin
			for (int i = 0; i < inputSin.Length; i++)
			{
				var sinDigit = sinNumbers[i];
				var controlNumber = controlList[i];
				int multiplicationResult = MultiplySinNumberToTheControlNumber(sinDigit, controlNumber);

				multiplicationResults.Add(multiplicationResult);
			}

			int finalSum = AddAllNumbers(multiplicationResults);
			int controlDigit = GetControlDigit(finalSum);

			return controlDigit;
		}

		private int GetControlDigit(int finalSum)
		{
			// Sum of the array should be evenly divisible by 10.
			// "10 - last digit" of the sum gives us the desired control digit. If last digit is 0, leaving as is.
			int secondDigitIndex = 1;
			var secondDigitOfSum = finalSum.ToString()[secondDigitIndex];
			int lastDigit = (int)char.GetNumericValue(secondDigitOfSum);
			bool lastDigitIsNotValid = lastDigit != 0;
			int controlDigit = 0;

			if (lastDigitIsNotValid)
			{
				controlDigit = 10 - lastDigit;
			}

			return controlDigit;
		}

		private int AddAllNumbers(List<int> resultNumbers)
		{
			int finalSum = 0;
			foreach (var number in resultNumbers)
			{
				finalSum += number;
			}

			return finalSum;
		}

		private int MultiplySinNumberToTheControlNumber(int sinNumber, int digitNumber)
		{
			int multiplicationResult = sinNumber * digitNumber;

			// If number is more than 1 digit (10-18 in our case) we sum first and second digit. (-9 gives the same result)
			bool currentValueIsTwoDigitNumber = multiplicationResult > 9;

			if (currentValueIsTwoDigitNumber)
			{
				multiplicationResult = multiplicationResult - 9;
			}

			return multiplicationResult;
		}

		private List<int> SplitSinIntoIntegers(string inputSin)
		{
			var sinNumbers = new List<int>();

			// Passing SIN string to the list
			foreach (char sinNumber in inputSin)
			{
				var numericCharacter = (int)char.GetNumericValue(sinNumber);
				sinNumbers.Add(numericCharacter);
			}

			return sinNumbers;
		}
	}
}
