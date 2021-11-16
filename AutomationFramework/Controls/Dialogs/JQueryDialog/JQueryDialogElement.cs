using System;
using System.Collections.Generic;
using System.Linq;

using AutomationFramework.Core.Attributes;
using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Engine;
using AutomationFramework.Core.Extensions;

using OpenQA.Selenium;

using By = AutomationFramework.Core.Engine.By;

namespace AutomationFramework.Core.Controls.Dialogs.JQueryDialog
{
	/// <summary>;
	/// Common jQuery dialog
	/// </summary>
	[Name("jQuery Dialog")]
	public class JQueryDialogElement : UiElement, ITitle
	{
		private readonly UiElement _dialog;

		public JQueryDialogElement(IWebElement webElement)
			: base(webElement)
		{
			_dialog = webElement.As<UiElement>();
		}

		public virtual string Title => GetVisibleHeaderTitle();

		public virtual void ClickButton(Enum button) => GetButtonByName(button).Click();

		internal string GetVisibleHeaderTitle()
		{
			int headerCount = Titles().Count - 1;
			for (int i = headerCount; i >= 0; i--)
			{
				if (Titles()[i].IsVisible)
				{
					return Titles()[i].Text;
				}
			}

			return string.Empty;
		}

		/// <summary>
		/// Gets button found by it's text
		/// </summary>
		/// <param name="button">Button name</param>
		/// <returns>IButton instance</returns>
		protected virtual IButton GetButtonByName(Enum button)
		{
			string buttonText = button.GetDescription();
			ButtonElement buttonElement = _dialog
				.FindElements<ButtonElement>(
					By.XPath($".//button[normalize-space(.)='{buttonText}']|.//span[normalize-space(.)='{buttonText}']//ancestor::button"))
				.FirstOrDefault(element => element.IsVisible);
			if (buttonElement is null)
			{
				string errorMessage = $"Button with text '{buttonText}' is not found in dialog footer.";
				Log.Error(errorMessage);

				throw new NotFoundException(errorMessage);
			}

			return buttonElement;
		}

		private List<SpanElement> Titles() =>
			Browser.FindElements<SpanElement>(By.XPath("//div[contains(@class,'ui-dialog-titlebar ui-widget-header')]/span"));
	}
}