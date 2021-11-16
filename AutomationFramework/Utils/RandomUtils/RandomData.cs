namespace AutomationFramework.Core.Utils.RandomUtils
{
	/// <summary>
	/// Helper class for getting the randomized test data
	/// </summary>
	public static class RandomData
	{
		/// <summary>
		/// Helper class for personal information
		/// </summary>
		public static Person Person => new Person();

		/// <summary>
		/// Helper class for accessing the Address specific information
		/// </summary>
		public static Address Address => new Address();

		/// <summary>
		/// Helper class for accessing the internet-specific information (random email address, username, etc)
		/// </summary>
		public static Internet Internet => new Internet();

		/// <summary>
		/// Helper class for the text content
		/// </summary>
		public static Lorem Lorem => new Lorem();

		/// <summary>
		/// Helper class for numeric values
		/// </summary>
		public static Numbers Numbers => new Numbers();

		/// <summary>
		/// Helper class for company and corporation content
		/// </summary>
		public static Company Company => new Company();
	}
}
