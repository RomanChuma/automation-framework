using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Extensions;

using OpenQA.Selenium;

using By = AutomationFramework.Core.Engine.By;

namespace AutomationFramework.Core.Controls.Dialogs.NgDialog
{
	public class NgDialogHeader : UiElement, ITextContent
	{
		private readonly IWebElement _header;

		internal NgDialogHeader(IWebElement webElement)
			: base(webElement)
		{
			_header = webElement;
		}

		public ISpan SpanDate => _header.As<UiElement>().FindElement<SpanElement>(By.XPath(".//span[contains(@class, 'date')]"));

		public string Text => _header.Text;

		public string Value => _header.GetAttribute("value");

		internal ISpan SpanTitle => _header.As<UiElement>().FindElement<SpanElement>(By.XPath(".//span[contains(@class, 'title')]"));
	}
}
