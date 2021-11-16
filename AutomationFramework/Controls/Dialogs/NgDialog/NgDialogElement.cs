using System;
using System.Linq;

using AutomationFramework.Core.Attributes;
using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Extensions;
using AutomationFramework.Core.Utils;

using OpenQA.Selenium;

using By = AutomationFramework.Core.Engine.By;

namespace AutomationFramework.Core.Controls.Dialogs.NgDialog
{
	/// <summary>
	/// Common Angular.js dialog element
	/// </summary>
	public class NgDialogElement : UiElement, IDialogWithFooter, ITitle, IDialogMessage
	{
		private readonly UiElement _dialog;

		public NgDialogElement(IWebElement webElement)
			: base(webElement)
		{
			_dialog = webElement.As<UiElement>();
		}

		public virtual NgDialogFooter Footer =>
			_dialog.FindElements<NgDialogFooter>(By.XPath(".//footer[contains(@class,'ngdialog-buttons')]")).FirstOrDefault();

		public virtual NgDialogHeader Header => _dialog.FindElements<NgDialogHeader>(By.XPath(".//header")).FirstOrDefault();

		public string Message => Body.SpanMessage.Text;

		public string Title => Header.Text;

		[Name("Close icon")]
		public IDiv XIcon => _dialog.FindElements<DivElement>(By.XPath(".//div[@class='ngdialog-close']")).FirstOrDefault();

		internal NgDialogBody Body => _dialog.FindElements<NgDialogBody>(By.XPath(".//section")).FirstOrDefault();

		public void ClickButton(Enum button)
		{
			try
			{
				IButton buttonElement = Footer.GetButton(button);
				buttonElement.Click();
			}
			catch (Exception e)
			{
				string errorMessage = $"Was not able to click on the button '{button.GetDescription()}'";
				Log.Error(errorMessage, e);
				throw;
			}
		}

		public void ClickOnCloseIcon() => XIcon.Click();

		public IButton GetButton(Enum buttonName) => Footer.GetButton(buttonName);

		/// <summary>
		/// Wait for dialog title panel to be displayed
		/// </summary>
		public void WaitForDialogTitlePanelToBeDisplayed()
		{
			Log.Trace("Waiting for dialog title to be displayed");

			try
			{
				Wait.Until(() => Header.SpanTitle.IsVisible);
			}
			catch
			{
				var errorMessage = "Dialog title did not appeared";
				Log.Error(errorMessage);

				throw new TimeoutException(errorMessage);
			}
		}

		/// <summary>
		/// Wait for dialog Title element to be loaded and contain text
		/// </summary>
		/// <param name="title">
		/// The title.
		/// </param>
		public void WaitForDialogTitleToLoadAndContainText(Enum title)
		{
			WaitForDialogTitlePanelToBeDisplayed();

			string titleText = title.GetDescription();
			Header.SpanTitle.WaitForTextToBePresent(titleText);
		}
	}
}