using System;
using System.Diagnostics;

using AutomationFramework.Core.Utils.Log;

namespace AutomationFramework.Core.Utils.Performance
{
	/// <summary>
	/// Helper class to profile and debug action methods
	/// </summary>
	public static class PerformanceHelper
	{
		private static readonly ILogger Log = Log4NetLogger.Instance;

		/// <summary>
		/// The timer
		/// </summary>
		private static readonly Stopwatch Timer = new Stopwatch();

		/// <summary>
		/// Profile action method
		/// </summary>
		/// <param name="action">The action to measure</param>
		public static void Profile(Action action)
		{
			StartMeasure();
			action();
			StopMeasure(action.Method.Name);
		}

		/// <summary>
		/// Starts the measure
		/// </summary>
		public static void StartMeasure()
		{
			Timer.Reset();
			Timer.Start();
		}

		/// <summary>
		/// Stops the measure
		/// </summary>
		/// <param name="actionTitle">
		/// The action Title
		/// </param>
		public static void StopMeasure(string actionTitle)
		{
			Timer.Stop();
			Log.Info($"Execution time of {actionTitle}: {Timer.Elapsed.Minutes} minutes {Timer.Elapsed.Seconds} seconds {Timer.Elapsed.Milliseconds} ms");
		}
	}
}