using System;
using System.Linq;

using AutomationFramework.Core.Attributes;
using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Extensions;

using OpenQA.Selenium;

using By = AutomationFramework.Core.Engine.By;

namespace AutomationFramework.Core.Controls.Dialogs.NgDialog
{
	/// <summary>
	/// Common Ng confirmation dialog element for Angular.js
	/// </summary>
	public class NgConfirmationDialogElement : UiElement, IDialogWithFooter, ITitle
	{
		private readonly UiElement _confirmationDialog;

		public NgConfirmationDialogElement(IWebElement webElement)
			: base(webElement)
		{
			_confirmationDialog = webElement.As<UiElement>();
		}

		public NgConfirmationDialogFooter Footer =>
			_confirmationDialog.FindElements<NgConfirmationDialogFooter>(By.XPath(".//footer")).FirstOrDefault();

		public string Title => _confirmationDialog.FindElement<LabelElement>(By.XPath(".//header")).Text;

		[Name("Close icon")]
		public IDiv XIcon => _confirmationDialog.FindElements<DivElement>(By.XPath(".//div[@class='ngdialog-close']")).FirstOrDefault();

		public void ClickButton(Enum button)
		{
			IButton buttonToBeClicked = Footer.GetButton(button);
			buttonToBeClicked.Click();
		}

		public void ClickOnCloseIcon() => XIcon.Click();

		public IButton GetButton(Enum buttonName) => Footer.GetButton(buttonName);
	}
}