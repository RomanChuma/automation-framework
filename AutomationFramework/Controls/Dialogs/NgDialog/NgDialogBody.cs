using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Extensions;

using OpenQA.Selenium;

using By = AutomationFramework.Core.Engine.By;

namespace AutomationFramework.Core.Controls.Dialogs.NgDialog
{
	internal class NgDialogBody : SectionElement
	{
		private readonly UiElement _body;

		internal NgDialogBody(IWebElement webElement) : base(webElement)
		{
			_body = webElement.As<UiElement>();
		}

		internal ISpan SpanMessage => _body.FindElement<SpanElement>(By.XPath(".//span"));
	}
}
