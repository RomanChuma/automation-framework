using System;

namespace AutomationFramework.Core.Utils.VideoRecorder
{
	public interface IVideoRecorder : IDisposable
	{
		string Record(string videoOutputDirectory, string fileName);

		void Stop();
	}
}