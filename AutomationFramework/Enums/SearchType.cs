namespace AutomationFramework.Core.Enums
{
	/// <summary>
	/// The page element locator search type. Needs to be translated to automation framework specific locators
	/// </summary>
	public enum SearchType
	{
		Id,
		IdEndingWith,
		ValueEndingWith,
		IdContaining,
		Tag,
		ClassName,
		XPath,
		CssClassContaining,
		LinkTextContaining,
		LinkText,
		XPathContaining,
		CssSelector,
		Name,
		InnerTextContains,
		NameEndingWith
	}
}
