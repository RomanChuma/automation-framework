using System.Collections.Generic;

namespace AutomationFramework.Core.Controls.Interfaces
{
	public interface IUnorderedList : IHtmlElement
	{
		List<ListItemElement> Items { get; }

		ListItemElement this[int index] { get; }

		ListItemElement this[string textValue] { get; }

		List<string> GetItemsText();
	}
}