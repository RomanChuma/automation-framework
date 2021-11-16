using System.Collections.Generic;

using AutomationFramework.Core.Controls.Interfaces;

namespace AutomationFramework.Core.Engine
{
	public partial class Browser
	{
		private static readonly ElementSearchService ElementSearch = new ElementSearchService();

		public static TElement FindElement<TElement>(By by)
			where TElement : class, IHtmlElement =>
			ElementSearch.FindElement<TElement>(Instance, by);

		public static List<TElement> FindElements<TElement>(By by)
			where TElement : class, IHtmlElement =>
			ElementSearch.FindElements<TElement>(Instance, by);
	}
}