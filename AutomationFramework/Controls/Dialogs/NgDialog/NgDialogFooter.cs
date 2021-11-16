using System;
using System.Linq;

using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Extensions;
using AutomationFramework.Core.Utils.Log;

using OpenQA.Selenium;

using By = AutomationFramework.Core.Engine.By;

namespace AutomationFramework.Core.Controls.Dialogs.NgDialog
{
	public class NgDialogFooter : SectionElement
	{
		private readonly UiElement _footer;

		private readonly ILogger _log = Log4NetLogger.Instance;

		internal NgDialogFooter(IWebElement webElement)
			: base(webElement)
		{
			_footer = webElement.As<UiElement>();
		}

		/// <summary>
		/// Gets button found by it's text
		/// </summary>
		/// <param name="buttonName">Button name</param>
		/// <returns>IButton instance</returns>
		public IButton GetButton(Enum buttonName)
		{
			string buttonText = buttonName.GetDescription();
			ButtonElement buttonElement = _footer
				.FindElements<ButtonElement>(By.XPath($".//button[.='{buttonText}']|.//span[.='{buttonText}']//ancestor::button"))
				.FirstOrDefault();

			if (buttonElement is null)
			{
				string errorMessage = $"Button with text '{buttonText}' is not found in dialog footer.";
				_log.Error(errorMessage);

				throw new NotFoundException(errorMessage);
			}

			return buttonElement;
		}
	}
}