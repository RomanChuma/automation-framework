namespace AutomationFramework.Core.Enums
{
	/// <summary>
	/// UI action type
	/// </summary>
	public enum ActionType
	{
		/// <summary>
		/// Use browser command via default Selenium Webdriver implementation
		/// </summary>
		Default,

		/// <summary>
		/// Use physical Mouse via Advanced Actions API
		/// </summary>
		Mouse,

		/// <summary>
		/// Use native JavaScript
		/// </summary>
		JavaScript,

		/// <summary>
		/// Use keyboard actions
		/// </summary>
		Keyboard
	}
}