using System;
using System.Linq;

using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Extensions;

using OpenQA.Selenium;

using By = AutomationFramework.Core.Engine.By;

namespace AutomationFramework.Core.Controls.Dialogs.JQueryDialog
{
	public class JQueryConfirmationDialogElement : JQueryDialogElement
	{
		private readonly UiElement _dialog;

		public JQueryConfirmationDialogElement(IWebElement webElement)
			: base(webElement)
		{
			_dialog = webElement.As<UiElement>();
		}

		public override void ClickButton(Enum button) => GetButtonByName(button).Click();

		public override string Title => DialogTitle.Text;

		public string ConfirmationText => DialogConfirmationText.Text;

		protected override IButton GetButtonByName(Enum button)
		{
			string buttonName = button.GetDescription();
			var buttonByName = _dialog.FindElements<ButtonElement>(By.XPath($".//button//span[contains(text(),'{buttonName}')]"))
			                          .FirstOrDefault();
			return buttonByName;
		}

		private ISpan DialogTitle =>
			_dialog.FindElements<SpanElement>(By.XPath("//div[contains(@class,'ui-dialog-titlebar ui-corner-all ui-widget-header')]/span")).FirstOrDefault(dialog => dialog.IsVisible);

		private ISpan DialogConfirmationText =>
			_dialog.FindElements<SpanElement>(By.XPath("//span[@id='confirmMessage']")).FirstOrDefault(dialog => dialog.IsVisible);
	}
}