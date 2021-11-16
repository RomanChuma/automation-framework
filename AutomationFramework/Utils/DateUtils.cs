using AutomationFramework.Core.Utils.Log;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AutomationFramework.Core.Utils
{
	public static class DateUtils
	{
		private static readonly ILogger Log = Log4NetLogger.Instance;

		/// <summary>
		/// DateTime.Now in a given string format
		/// </summary>
		/// <param name="format">String representation, by default is set to: "HH:mm:ss"</param>
		/// <returns>String</returns>
		public static string GetCurrentTime(string format = "HH:mm:ss") => DateTime.Now.ToString(format);

		/// <summary>
		/// DateTime.Today in a given string format
		/// </summary>
		/// <param name="format">String representation, it is possible to set output format (Ex: "dd-MM-yyyy", "yyyy-MM-dd" etc.)</param>
		/// <returns>String</returns>
		public static string GetCurrentDateInFormat(string format) => DateTime.Today.ToString(format);

		/// <summary>
		/// DateTime.Today in a given string format
		/// </summary>
		/// <param name="format">String representation of date time in default format (Ex: "ddd MMMM d yyyy HH:mm:ss + 'CurrentCulture'")</param>
		/// <returns>String</returns>
		public static string GetCurrentDate(string format = "ddd MMMM d") => DateTime.Today.ToString(format) + " " + GetCurrentYear() + " " + DateTime.Now.ToString("HH:mm:ss") + " " + Thread.CurrentThread.CurrentCulture;

		/// <summary>
		/// Returns current month int representation (Ex: if current month order number is equal to '5', then method returns '5')
		/// </summary>
		/// <returns>int</returns>
		public static int GetCurrentMonthOrderNumber() => int.Parse(DateTime.Now.Month.ToString());

		/// <summary>
		/// Returns previous month int representation (Ex: if current month order number is equal to '5', then method returns '4')
		/// </summary>
		/// <returns>int</returns>
		public static int GetPreviousMonthOrderNumber() => GetCurrentMonthOrderNumber() - 1;

		/// <summary>
		/// Returns next month int representation (Ex: if current month order number is equal to '5', then method returns '6')
		/// </summary>
		/// <returns>int</returns>
		public static int GetNextMonthOrderNumber() => GetCurrentMonthOrderNumber() + 1;

		/// <summary>
		/// Returns current month string representation (Ex: 'May')
		/// </summary>
		/// <returns>String</returns>
		public static string GetCurrentMonth() => DateTime.Now.ToString("MMMM");

		/// <summary>
		/// Returns current year string representation (Ex: if current year is '2018', then method returns'2018')
		/// </summary>
		/// <returns>Int</returns>
		public static string GetCurrentYear() => DateTime.Now.Year.ToString();

		/// <summary>
		/// Returns next year string representation (Ex: if current year is '2018', then method returns '2019')
		/// </summary>
		/// <returns>String</returns>
		public static string GetNextYear() => (DateTime.Now.Year + 1).ToString();

		/// <summary>
		/// Returns previous year string representation (Ex: if current year is '2018', then method returns '2017')
		/// </summary>
		/// <returns>String</returns>
		public static string GetPreviousYear() => (DateTime.Now.Year - 1).ToString();

		/// <summary>
		/// Returns Date time in milliseconds long representation
		/// </summary>
		/// <returns>long</returns>
		public static double GenerateDateTimeInMilliseconds()
		{
			long time = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

			return time;
		}

		/// <summary>
		/// Get Duration from two times
		/// </summary>
		/// <param name="startTime">startTime for finding duration (8:00 AM)</param>
		/// <param name="endTime">endTime for finding Duration (9:00 AM)</param>
		/// <returns>Duration in hours</returns>
		public static string GetDurationInHours(string startTime, string endTime)
		{
			DateTime.TryParse(startTime, out DateTime parsedStartTime);
			DateTime.TryParse(endTime, out DateTime parsedEndTime);

			TimeSpan duration = parsedEndTime - parsedStartTime;
			var format = "0.0000";
			var durationInHours = duration.TotalHours.ToString(format);

			return durationInHours;
		}

		/// <summary>
		/// Convert string date to DateTime format
		/// </summary>
		/// <param name="dateString">Date in string format</param>
		/// <returns>Converted date</returns>
		public static DateTime ConvertStringToDateTime(string dateString)
		{
			bool inputIsNotValid = !DateTime.TryParse(dateString, out DateTime dateOutput);

			if (inputIsNotValid)
			{
				Log.Error("Error occurred during conversion of string to date");
			}

			return dateOutput;
		}

		/// <summary>
		/// Gets the list of dateTime in provided range of seconds. Is used for timestamps verifications
		/// Adds values from minus rangeMinutes to plus rangeMinutes
		/// </summary>
		/// <param name="currentTime">Current dateTime</param>
		/// <param name="rangeMinutes">Range of minutes</param>
		/// <returns>List of DateTime</returns>
		public static List<DateTime> GetTimestampRange(DateTime currentTime, int rangeMinutes)
		{
			var resultRange = new List<DateTime>();

			for (var i = -rangeMinutes; i < rangeMinutes; i++)
			{
				resultRange.Add(currentTime.AddMinutes(i));
			}

			return resultRange;
		}

		/// <summary>
		/// Calculate a person's age from a birthdate string provided.
		/// </summary>
		/// <param name="birthDate">Birthday string date</param>
		/// <returns>age in int</returns>
		public static int CalculateAgeFromDateTime(DateTime birthDate)
		{
			DateTime today = DateTime.Today;

			int age = today.Year - birthDate.Year;

			if (birthDate.Date > today.AddYears(-age))
			{
				age--;
			}

			return age;
		}

		/// <summary>
		///  Returns date by adding months
		/// </summary>
		/// <param name="dateTime">date to add</param>
		/// <param name="monthsToAdd">Months to add</param>
		/// <param name="format">String representation of date time</param>
		/// <returns>Returns date by adding months specified in the parameter(Default is 0)</returns>
		public static string AddMonthsToDate(DateTime dateTime, int monthsToAdd = 0, string format = "yyyy-MM-dd")
		{
			var date = dateTime;
			var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
			string newDate;

			if (date.Day != daysInMonth)
			{
				newDate = date.AddMonths(monthsToAdd).ToString(format);
				return newDate;
			}

			newDate = date.AddDays(1).AddMonths(monthsToAdd).AddDays(-1).ToString(format);
			return newDate;
		}
	}
}
