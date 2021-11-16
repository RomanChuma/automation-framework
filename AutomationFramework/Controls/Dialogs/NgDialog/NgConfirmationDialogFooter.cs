using System;

using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Extensions;

using OpenQA.Selenium;

namespace AutomationFramework.Core.Controls.Dialogs.NgDialog
{
	public class NgConfirmationDialogFooter : SectionElement
	{
		private readonly IWebElement _footer;

		internal NgConfirmationDialogFooter(IWebElement webElement) : base(webElement)
		{
			_footer = webElement;
		}

		public IButton GetButton(Enum buttonName)
		{
			string buttonNameValue = buttonName.GetDescription();
			var button = _footer.FindElement(By.XPath($".//button[.='{buttonNameValue}']")).As<ButtonElement>();
			return button;
		}
	}
}
