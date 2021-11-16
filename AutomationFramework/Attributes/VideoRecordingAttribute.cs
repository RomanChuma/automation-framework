using System;

using AutomationFramework.Core.Utils.VideoRecorder;

namespace AutomationFramework.Core.Attributes
{
	/// <summary>
	/// Applies the test video recording mode
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public sealed class VideoRecordingAttribute : Attribute
	{
		public VideoRecordingAttribute(VideoRecordingMode videoRecordingMode)
		{
			VideoRecording = videoRecordingMode;
		}

		public VideoRecordingMode VideoRecording { get; set; }
	}
}