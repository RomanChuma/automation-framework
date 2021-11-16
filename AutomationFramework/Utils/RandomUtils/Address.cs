namespace AutomationFramework.Core.Utils.RandomUtils
{
	public class Address
	{
		internal Address()
		{
		}

		public string City => new Bogus.DataSets.Address().City();

		public string Country => new Bogus.DataSets.Address().Country();

		public string StreetAddress => new Bogus.DataSets.Address().StreetAddress();

		public string StreetName => new Bogus.DataSets.Address().StreetName();

		public string BuildingNumber => new Bogus.DataSets.Address().BuildingNumber();

		public string ZipCode(string format = null) => new Bogus.DataSets.Address().ZipCode(format);
	}
}
