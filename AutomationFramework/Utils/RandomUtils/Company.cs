namespace AutomationFramework.Core.Utils.RandomUtils
{
	public class Company
	{
		internal Company()
		{
		}

		public string Bs => new Bogus.DataSets.Company().Bs();

		public string CompanyName => new Bogus.DataSets.Company().CompanyName();

		public string BusinessNumber => RandomData.Numbers.GetRandomNumbers(9).ToString();
	}
}
