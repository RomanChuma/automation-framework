namespace AutomationFramework.Core.Utils.VideoRecorder
{
	/// <summary>
	/// Text execution recording mode
	/// </summary>
	public enum VideoRecordingMode
	{
		/// <summary>
		/// Record always, disregard the test outcome
		/// </summary>
		Always,

		/// <summary>
		/// Do not record
		/// </summary>
		DoNotRecord,

		/// <summary>
		/// Ignore video recording (used for system purposes)
		/// </summary>
		Ignore,

		/// <summary>
		/// Record only tests that have passed
		/// </summary>
		OnlyPassed,

		/// <summary>
		/// Record only tests that have failed
		/// </summary>
		OnlyFailed
	}
}