using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Enums;

namespace AutomationFramework.Core.Engine
{
	public class By
	{
		public By(SearchType type, string value) : this(type, value, null)
		{
		}

		public By(SearchType type, string value, IHtmlElement parent)
		{
			Type = type;
			Value = value;
			Parent = parent;
		}

		public SearchType Type { get; private set; }

		public string Value { get; private set; }

		public IHtmlElement Parent { get; private set; }

		public static By ClassName(string className) => new By(SearchType.ClassName, className);

		public static By Id(string id) => new By(SearchType.Id, id);

		public static By Id(string id, IHtmlElement parentElement) => new By(SearchType.Id, id, parentElement);

		public static By LinkText(string linkText) => new By(SearchType.LinkText, linkText);

		public static By Tag(string tagName) => new By(SearchType.Tag, tagName);

		public static By Tag(string tagName, IHtmlElement parentElement) => new By(SearchType.Tag, tagName, parentElement);

		public static By CssSelector(string cssSelector) => new By(SearchType.CssSelector, cssSelector);

		public static By CssSelector(string cssSelector, IHtmlElement parentElement) => new By(SearchType.CssSelector, cssSelector, parentElement);

		public static By Name(string name) => new By(SearchType.Name, name);

		public static By Name(string name, IHtmlElement parentElement) => new By(SearchType.Name, name, parentElement);

		public static By XPath(string xPath) => new By(SearchType.XPath, xPath);

		public static By XPath(string xPath, IHtmlElement parentElement) => new By(SearchType.XPath, xPath, parentElement);
	}
}
