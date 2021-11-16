using System;

using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Extensions;

using OpenQA.Selenium;

using By = AutomationFramework.Core.Engine.By;

namespace AutomationFramework.Core.Controls.Dialogs.JQueryDialog
{
	public class JQueryDialogFooter : SectionElement
	{
		private readonly UiElement _dialogFooter;

		public JQueryDialogFooter(IWebElement webElement)
			: base(webElement)
		{
			_dialogFooter = webElement.As<UiElement>();
		}

		public IButton GetButtonByName(Enum button)
		{
			string buttonName = button.GetDescription();
			return _dialogFooter.FindElement<ButtonElement>(
				By.XPath($"//div[contains(@id,'ButtonPanel')]//button[.='{buttonName}']"));
		}
	}
}