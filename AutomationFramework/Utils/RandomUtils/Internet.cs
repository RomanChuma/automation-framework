namespace AutomationFramework.Core.Utils.RandomUtils
{
	public class Internet
	{
		internal Internet()
		{
		}

		public string Password => new Bogus.DataSets.Internet().Password();

		public string GenerateUserName(string firstName = null, string lastName = null)
		{
			var userName = new Bogus.DataSets.Internet().UserName(firstName, lastName);
			return userName;
		}

		public string GetEmail(string firstName = null, string lastName = null)
		{
			var email = new Bogus.DataSets.Internet().Email(firstName, lastName);
			return email;
		}
	}
}