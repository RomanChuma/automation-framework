using System.Collections.Generic;
using System.Linq;

using AutomationFramework.Core.Controls.Interfaces;

using OpenQA.Selenium;

namespace AutomationFramework.Core.Controls
{
	public class UnorderedListElement : UiElement, IUnorderedList
	{
		public UnorderedListElement(IWebElement webElement)
			: base(webElement)
		{
		}

		public List<ListItemElement> Items => GetUnderlyingElements<ListItemElement>();

		public List<string> GetItemsText() => Items.Select(x => x.Text).ToList();

		public ListItemElement this[int index] => Items[index];

		public ListItemElement this[string textValue] => Items.FirstOrDefault(x => x.Text.Equals(textValue));
	}
}