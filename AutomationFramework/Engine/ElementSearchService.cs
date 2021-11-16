using System;
using System.Collections.Generic;

using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Extensions;
using AutomationFramework.Core.Utils.Log;

using OpenQA.Selenium;

namespace AutomationFramework.Core.Engine
{
	/// <summary>
	/// Class for extending the basic Selenium WebDriver search to be able find custom web elements
	/// </summary>
	public class ElementSearchService
	{
		private static readonly ILogger Log = Log4NetLogger.Instance;

		/// <summary>
		/// Find the first given custom web element
		/// </summary>
		/// <typeparam name="TElement">Custom web element type</typeparam>
		/// <param name="searchContext">SearchContext</param>
		/// <param name="by">Locator</param>
		/// <returns>Custom web element</returns>
		public TElement FindElement<TElement>(ISearchContext searchContext, By by)
			where TElement : class, IHtmlElement
		{
			try
			{
				var element = searchContext.FindElement(by.ToSeleniumBy());
				var result = element.As<TElement>();
				return result;
			}
			catch (NoSuchElementException e)
			{
				var customMessage =
					"Element is not found in page HTML. Verify that you are on the correct page or that the element is not located within the iFrame";
				Log.Error(e.Message, e);
				Log.Error(customMessage);
				throw new NoSuchElementException($"{customMessage}." + Environment.NewLine + e.Message);
			}
		}

		/// <summary>
		/// Find all custom web elements of given type
		/// </summary>
		/// <typeparam name="TElement">Custom web element type</typeparam>
		/// <param name="searchContext">SearchContext</param>
		/// <param name="by">Locator</param>
		/// <returns>List of custom web elements</returns>
		public List<TElement> FindElements<TElement>(ISearchContext searchContext, By by)
			where TElement : class, IHtmlElement
		{
			var elements = searchContext.FindElements(by.ToSeleniumBy());
			var resolvedElements = new List<TElement>();

			foreach (var currentElement in elements)
			{
				var result = currentElement.As<TElement>();
				resolvedElements.Add(result);
			}

			return resolvedElements;
		}
	}
}