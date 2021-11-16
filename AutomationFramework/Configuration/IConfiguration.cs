using System.Configuration;

namespace AutomationFramework.Core.Configuration
{
	public interface IConfiguration
	{
		ConfigurationSection Initialize();
	}
}
