using System;

using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Engine;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace AutomationFramework.Core.Controls
{
	public class LabelElement : UiElement, ILabel
	{
		private readonly IWebElement _label;

		public LabelElement(IWebElement webElement) : base(webElement)
		{
			_label = webElement;
		}

		public string Text => _label.Text;

		public string Value => _label.GetAttribute("value");

		/// <summary>
		/// Wait for text to be present in label element
		/// </summary>
		/// <param name="elementText">Text to wait</param>
		/// <param name="timeOut">Timeout, seconds</param>
		public void WaitForTextToBePresent(string elementText, int timeOut = 20)
		{
			var wait = new WebDriverWait(Browser.Instance, TimeSpan.FromSeconds(timeOut));
			Log.Trace($"Waiting for {OuterHtml} element to contain text '{elementText}' on '{Browser.Title}' page");

			try
			{
				wait.Until(ExpectedConditions.TextToBePresentInElement(_label, elementText));
			}
			catch
			{
				var errorMessage = $"'{elementText}' did not appeared within element."
								   + Environment.NewLine
								   + $"Actual element text is: '{_label.Text}'";
				Log.Error(errorMessage);

				throw new TimeoutException(errorMessage);
			}

			Log.Trace($"'{elementText}' text appeared");
		}
	}
}
