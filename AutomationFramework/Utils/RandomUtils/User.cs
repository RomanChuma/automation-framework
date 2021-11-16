using System;

namespace AutomationFramework.Core.Utils.RandomUtils
{
	/// <summary>
	/// Representation of User API Object from Firm Partner API
	/// </summary>
	public class User
	{
        public static string Id => Guid.NewGuid().ToString();
		public string Name => new Bogus.Person().FullName;

        public bool Partner => false;

        public bool Manager => false;
		public string UserName => new Bogus.Person().UserName;
		public string FirstName => new Bogus.Person().FirstName;
        public string LastName => new Bogus.Person().LastName;
        public string Email => new Bogus.Person().Email;
    }
}
