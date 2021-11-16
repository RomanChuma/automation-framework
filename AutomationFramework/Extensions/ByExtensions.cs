using System;

using AutomationFramework.Core.Enums;

using OpenQA.Selenium;

namespace AutomationFramework.Core.Extensions
{
	public static class ByExtensions
	{
		public static By ToSeleniumBy(this Engine.By by)
		{
			switch (by.Type)
			{
				case SearchType.Id:
					return By.Id(by.Value);
				case SearchType.Tag:
					return By.TagName(by.Value);
				case SearchType.ClassName:
					return By.ClassName(by.Value);
				case SearchType.LinkText:
					return By.LinkText(by.Value);
				case SearchType.LinkTextContaining:
					return By.PartialLinkText(by.Value);
				case SearchType.XPath:
					return By.XPath(by.Value);
				case SearchType.CssSelector:
					return By.CssSelector(by.Value);
				case SearchType.Name:
					return By.Name(by.Value);
				default:
					throw new NotSupportedException(string.Format($"Unknown search type: '{by.Type}'"));
			}
		}
	}
}
