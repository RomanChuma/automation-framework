using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Extensions;

using OpenQA.Selenium;

using By = AutomationFramework.Core.Engine.By;

namespace AutomationFramework.Core.Controls.DevExpress
{
	public class ASPxRteContextMenu : UiElement, IUnorderedList
	{
		private readonly UiElement _contextMenu;

		public ASPxRteContextMenu(IWebElement webElement)
			: base(webElement)
		{
			_contextMenu = webElement.As<UiElement>();
		}

		public List<ListItemElement> Items => _contextMenu.FindElements<ListItemElement>(By.XPath(".//li")).ToList();

		public ListItemElement this[int index] => Items[index];

		public ListItemElement this[string textValue] => Items.FirstOrDefault(x => x.Text.Equals(textValue));

		public List<string> GetItemsText() => Items.Select(x => x.Text).ToList();
	}
}
