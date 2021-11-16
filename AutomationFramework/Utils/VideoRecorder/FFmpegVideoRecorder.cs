using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Utils.Log;

namespace AutomationFramework.Core.Utils.VideoRecorder
{
	/// <summary>
	/// Implementation of video recorder using FFmpeg library
	/// </summary>
	public class FFmpegVideoRecorder : IVideoRecorder
	{
		private readonly ILogger _log = Log4NetLogger.Instance;

		private Process _recorderProcess;

		private bool _videoRecordingIsRunning;

		public void Dispose()
		{
			if (_videoRecordingIsRunning)
			{
				// Wait for 500 milliseconds before finishing video
				Wait.For(TimeSpan.FromMilliseconds(500));

				if (!_recorderProcess.HasExited)
				{
					_log.Debug("Killing FFmpeg recorder process");
					_recorderProcess?.Kill();
					_recorderProcess?.WaitForExit();
				}

				_videoRecordingIsRunning = false;
			}
		}

		/// <summary>
		/// Start video recording
		/// </summary>
		/// <param name="videoOutputDirectory">Path to recorded video</param>
		/// <param name="fileName">Video file name</param>
		/// <returns>Path to recorded video file</returns>
		public string Record(string videoOutputDirectory, string fileName)
		{
			string videoFilePathWithExtension = $"{Path.GetFileNameWithoutExtension(fileName)}.avi";
			string videoPath = $"{Path.Combine(videoOutputDirectory, videoFilePathWithExtension)}";

			try
			{
				if (!Directory.Exists(videoOutputDirectory))
				{
					_log.Trace($"Created directory for video recorder: {videoOutputDirectory}");
					Directory.CreateDirectory(videoOutputDirectory);
				}
			}
			catch (Exception ex)
			{
				throw new ArgumentException(
					$"A problem occurred trying to initialize the create the directory you have specified. - {videoOutputDirectory}",
					ex);
			}

			if (!_videoRecordingIsRunning)
			{
				ProcessStartInfo startInfo = GetProcessStartInfo(videoPath);

				_recorderProcess = new Process { StartInfo = startInfo, EnableRaisingEvents = true };
				_recorderProcess.Start();
				_recorderProcess.BeginErrorReadLine();

				_videoRecordingIsRunning = true;
				_log.Debug($"Started recording video '{videoPath}'");
			}

			return videoPath;
		}

		public void Stop()
		{
			_log.Debug("Stop the video recorder");
			Dispose();
		}

		private string GetFFmpegPath()
		{
			string executingAssemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var ffmpegExecutablePath = "ffmpeg.exe";
			string combinedPath = Path.Combine(executingAssemblyLocation, ffmpegExecutablePath);
			return combinedPath;
		}

		private ProcessStartInfo GetProcessStartInfo(string videoFilePathWithExtension)
		{
			var startInfo = new ProcessStartInfo
				                {
					                FileName = GetFFmpegPath(),
					                RedirectStandardInput = true,
					                RedirectStandardOutput = true,
					                RedirectStandardError = true,
					                UseShellExecute = false,
					                CreateNoWindow = false,
					                Arguments =
						                $"{GlobalSettings.Framework.FFmpegArguments} {videoFilePathWithExtension}"
				                };

			return startInfo;
		}
	}
}