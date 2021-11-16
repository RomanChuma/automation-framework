namespace AutomationFramework.Core.Utils.Jenkins
{
	public sealed class Jenkins
	{
		private static readonly object CheckLock = new object();

		private static Jenkins instance;

		private Jenkins()
		{
			ApiClient = new JenkinsApiClient();
		}

		public static Jenkins Instance
		{
			get
			{
				lock (CheckLock)
				{
					if (instance == null)
					{
						instance = new Jenkins();
					}

					return instance;
				}
			}
		}

		public JenkinsApiClient ApiClient { get; set; }
	}
}