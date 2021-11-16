namespace AutomationFramework.Core.Utils.RandomUtils
{
	public class Finance
	{
		public string AccountNumber => new Bogus.DataSets.Finance().Account();

		public string AccountName => new Bogus.DataSets.Finance().AccountName();

		public string CreditCardNumber => new Bogus.DataSets.Finance().CreditCardNumber();
	}
}
