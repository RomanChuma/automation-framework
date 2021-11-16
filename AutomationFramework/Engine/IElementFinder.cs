using System.Collections.Generic;

using AutomationFramework.Core.Controls.Interfaces;

namespace AutomationFramework.Core.Engine
{
	public interface IElementFinder
	{
		TElement FindElement<TElement>(By by) where TElement : class, IHtmlElement;

		IEnumerable<TElement> FindElements<TElement>(By by) where TElement : class, IHtmlElement;
	}
}
