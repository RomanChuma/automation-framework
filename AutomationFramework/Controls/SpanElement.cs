using System;

using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Engine;
using AutomationFramework.Core.Utils.Log;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace AutomationFramework.Core.Controls
{
	public class SpanElement : UiElement, ISpan
	{
		private readonly ILogger _log = Log4NetLogger.Instance;
		private readonly IWebElement _span;

		public SpanElement(IWebElement webElement) : base(webElement)
		{
			_span = webElement;
		}

		public string Text => _span.Text;

		public string Value => _span.GetAttribute("value");

		/// <summary>
		/// Wait for text to be present in label element
		/// </summary>
		/// <param name="elementText">Text to wait</param>
		/// <param name="timeOut">Timeout, seconds</param>
		public void WaitForTextToBePresent(string elementText, int timeOut = 20)
		{
			var wait = new WebDriverWait(Browser.Instance, TimeSpan.FromSeconds(timeOut));
			_log.Trace($"Waiting for {OuterHtml} element to contain text '{elementText}' on '{Browser.Title}' page");

			try
			{
				wait.Until(ExpectedConditions.TextToBePresentInElement(_span, elementText));
			}
			catch
			{
				var errorMessage = $"'{elementText}' did not appeared within span element."
								   + Environment.NewLine
								   + $"Actual span element text is: '{_span.Text}'";
				_log.Error(errorMessage);

				throw new TimeoutException(errorMessage);
			}

			_log.Trace($"'{elementText}' text appeared");
		}
	}
}
