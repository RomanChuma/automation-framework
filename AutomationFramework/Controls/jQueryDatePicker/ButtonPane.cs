using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Engine;

using OpenQA.Selenium;

using By = AutomationFramework.Core.Engine.By;

namespace AutomationFramework.Core.Controls.jQueryDatePicker
{
	public class ButtonPane : DivElement
	{
		/// <inheritdoc />
		public ButtonPane(IWebElement webElement)
			: base(webElement)
		{
		}

		public IButton BtnToday =>
			Browser.FindElement<ButtonElement>(
				By.XPath(
					"//button[@class='ui-datepicker-current ui-state-default ui-priority-secondary ui-corner-all' and @data-handler='today']"));

		public IButton BtnDone => Browser.FindElement<ButtonElement>(
			By.XPath(
				"//button[@data-handler='hide']"));
	}
}
