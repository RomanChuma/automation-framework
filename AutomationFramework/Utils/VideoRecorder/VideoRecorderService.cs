using System;
using System.IO;
using System.Reflection;
using System.Threading;
using AutomationFramework.Core.Attributes;
using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Utils.Log;

namespace AutomationFramework.Core.Utils.VideoRecorder
{
	public class VideoRecorderService
	{
		private readonly ILogger _log = Log4NetLogger.Instance;

		private static readonly ThreadLocal<IVideoRecorder> VideoRecorders = new ThreadLocal<IVideoRecorder>();

		private string _videoRecordingPath;

		public VideoRecorderService(IVideoRecorder videoRecorder)
		{
			VideoRecorders.Value = videoRecorder;
		}

		public VideoRecordingMode RecordingMode { get; private set; }

		public void Cleanup()
		{
			bool recordingIsEnabled = RecordingMode != VideoRecordingMode.DoNotRecord;

			if (recordingIsEnabled)
			{
				VideoRecorders.Value.Stop();

				TimeSpan retentionDuration = TimeSpan.FromDays(GlobalSettings.Framework.VideosRetentionDuration);
				FileHandler.DeleteFilesOlderThan(GlobalSettings.Framework.VideosDirectory, retentionDuration);

				if (GlobalSettings.Framework.DeleteVideoRecordsInCleanup)
				{
					DeleteRecordedVideo();
				}
			}
		}

		public void DeleteRecordedVideo()
		{
			if (RecordingMode == VideoRecordingMode.DoNotRecord)
			{
				return;
			}

			_log.Debug($"Delete the video recording '{_videoRecordingPath}'");

			try
			{
				FileHandler.DeleteFile(_videoRecordingPath);
			}
			catch (Exception e)
			{
				_log.Error($"Was not able to delete the video '{_videoRecordingPath}'", e);
			}
		}

		public string Record(string fileName)
		{
			string outputDirectory = GetOutputFolder();
			_videoRecordingPath = VideoRecorders.Value.Record(outputDirectory, fileName);
			return _videoRecordingPath;
		}

		public string StartTestRecording(MemberInfo memberInfo, string videoFileName)
		{
			RecordingMode = GetTestVideoRecordingMode(memberInfo);

			if (RecordingMode != VideoRecordingMode.DoNotRecord)
			{
				Record(videoFileName);
			}

			return _videoRecordingPath;
		}

		private string GetOutputFolder()
		{
			string outputDirectory = GlobalSettings.Framework.VideosDirectory;

			if (!Directory.Exists(outputDirectory))
			{
				Directory.CreateDirectory(outputDirectory);
			}

			return outputDirectory;
		}

		/// <summary>
		/// Gets test video recording mode from the memberInfo
		/// </summary>
		/// <param name="memberInfo">Test metadata</param>
		/// <returns><see cref="VideoRecordingMode"/> based on test or fixture attributes</returns>
		private VideoRecordingMode GetTestVideoRecordingMode(MemberInfo memberInfo)
		{
			var currentRecordingMode = VideoRecordingMode.DoNotRecord;

			bool videoRecordingIsDisabled = !GlobalSettings.Framework.TakeVideosOfTestExecution;

			if (videoRecordingIsDisabled)
			{
				return currentRecordingMode;
			}

			VideoRecordingMode testRecordingMode = GetVideoRecordingModeByMethodInfo(memberInfo);
			VideoRecordingMode fixtureRecordingMode = GetVideoRecordingModeByType(memberInfo.DeclaringType);

			if (testRecordingMode != VideoRecordingMode.Ignore)
			{
				currentRecordingMode = testRecordingMode;
			}
			else if (fixtureRecordingMode != VideoRecordingMode.Ignore)
			{
				currentRecordingMode = fixtureRecordingMode;
			}

			return currentRecordingMode;
		}

		private VideoRecordingMode GetVideoRecordingModeByMethodInfo(MemberInfo memberInfo)
		{
			if (memberInfo == null)
			{
				throw new ArgumentNullException(nameof(memberInfo), "Method info should be provided");
			}

			var recordingModeMethodAttribute = memberInfo.GetCustomAttribute<VideoRecordingAttribute>(true);

			if (recordingModeMethodAttribute != null)
			{
				return recordingModeMethodAttribute.VideoRecording;
			}

			return VideoRecordingMode.Ignore;
		}

		private VideoRecordingMode GetVideoRecordingModeByType(Type currentType)
		{
			if (currentType == null)
			{
				throw new ArgumentNullException(nameof(currentType), "Current type should be provided");
			}

			var recordingModeClassAttribute = currentType.GetCustomAttribute<VideoRecordingAttribute>(true);

			if (recordingModeClassAttribute != null)
			{
				return recordingModeClassAttribute.VideoRecording;
			}

			return VideoRecordingMode.Ignore;
		}
	}
}