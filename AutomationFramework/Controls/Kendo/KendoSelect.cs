using System;
using System.Collections.Generic;
using System.Linq;

using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Engine;
using AutomationFramework.Core.Extensions;
using AutomationFramework.Core.Utils;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;

using By = AutomationFramework.Core.Engine.By;

namespace AutomationFramework.Core.Controls.Kendo
{
	/// <summary>
	/// Kendo UI Select element
	/// </summary>
	public abstract class KendoSelect : UiElement, IKendoSelect
	{
		protected internal readonly IWebElement KendoSelectElement;

		protected readonly UiElement ParentComponent;

		private readonly string _parentComponentLocator = "//ancestor-or-self::span[contains(@class, 'k-widget')]";

		protected KendoSelect(IWebElement webElement)
			: base(webElement)
		{
			KendoSelectElement = webElement;
			ParentComponent = KendoSelectElement.As<UiElement>().FindElement<UiElement>(By.XPath(_parentComponentLocator));
		}

		public bool IsEnabled => KendoSelectElement.Enabled;

		public bool IsLoaded => IsVisible && SpinnerIsNotPresent;

		public bool IsOpened
		{
			get
			{
				string ariaExpandedValue = VisibleInput.GetAttribute("aria-expanded");
				var isExpanded = Convert.ToBoolean(ariaExpandedValue);
				return isExpanded;
			}
		}

		public override bool IsVisible => KendoSelectElement.Displayed;

		public string SelectedOption => (string)Browser.InvokeScript($"return $(arguments[0]).data('{SelectType}').text();", KendoSelectElement);

		public string Text => KendoSelectElement.Text;

		public string Value => KendoSelectElement.GetAttribute("value");

		public abstract int OptionsCount { get; }

		internal virtual IWebElement VisibleInput
		{
			get
			{
				object scriptResult = Browser.InvokeScript(
					$"return $(arguments[0]).data('{SelectType}').input.toArray()[0];",
					KendoSelectElement);
				return (IWebElement)scriptResult;
			}
		}

		/// <summary>Gets the selector.</summary>
		/// <value>The selector.</value>
		protected abstract string SelectType { get; }

		private UiElement LoadingSpinner =>
			KendoSelectElement.As<UiElement>().FindElements<UiElement>(By.XPath("//span[@class='k-icon k-i-arrow-60-down k-i-loading']"))
				.FirstOrDefault();

		private bool SpinnerIsNotPresent => LoadingSpinner == null;

		public void Close() => Browser.Instance.ExecuteJavaScript($"$(arguments[0]).data('{SelectType}').close();", KendoSelectElement);

		public abstract IEnumerable<string> GetOptions();

		public void Open() => Browser.Instance.ExecuteJavaScript($"$(arguments[0]).data('{SelectType}').open();", KendoSelectElement);

		public void SelectOptionByIndex(int index)
		{
			Browser.Instance.ExecuteJavaScript($"$(arguments[0]).data('{SelectType}').select({index});", KendoSelectElement);
			Browser.Instance.ExecuteJavaScript($"$(arguments[0]).data('{SelectType}').trigger('change')", KendoSelectElement);
		}

		public void SelectOptionByPartialMatch(string textValue)
		{
			var options = GetOptions().ToList();
			int index = -1;
			for (var i = 0; i < options.Count; i++)
			{
				if (options[i].Contains(textValue))
				{
					index = i;
				}
			}

			bool elementNotFound = index == -1;
			string errorMessage = $"Option, which contains '{textValue}' is not found";

			if (elementNotFound)
			{
				Log.Error(errorMessage);
				throw new NotFoundException(errorMessage);
			}

			SelectOptionByIndex(index);
		}

		public void SelectOptionByText(string textValue)
		{
			var options = GetOptions().ToList();
			int index = -1;

			for (var i = 0; i < options.Count; i++)
			{
				if (options[i].Equals(textValue))
				{
					index = i;
				}
			}

			bool elementNotFound = index == -1;
			string errorMessage = $"Option, which contains '{textValue}' is not found";

			if (elementNotFound)
			{
				Log.Error(errorMessage);
				throw new NotFoundException(errorMessage);
			}

			SelectOptionByIndex(index);
		}

		public void WaitUntilContentIsLoaded()
		{
			Wait.Until(() => IsLoaded);
		}
	}
}