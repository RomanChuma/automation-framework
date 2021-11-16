using System;
using System.Collections.Generic;

using AutomationFramework.Core.Controls;
using AutomationFramework.Core.Controls.Kendo;

using OpenQA.Selenium.Support.UI;

namespace AutomationFramework.Core.Constants
{
	/// <summary>
	/// Storage for key/value pairs of UiElement/XPath for usage in search
	/// </summary>
	public static class HtmlElementXPath
	{
		internal static readonly Dictionary<Type, string> XPath = new Dictionary<Type, string>
		{
			{ typeof(AnchorElement), "//a" },
			{ typeof(SelectElement), "//select" },
			{ typeof(SpanElement), "//span" },
			{ typeof(DivElement), "//div" },
			{ typeof(InputTextElement), "//input[@type='text']" },
			{ typeof(ButtonElement), "//button" },
			{ typeof(InputCheckboxElement), "//input[@type='checkbox']" },
			{ typeof(ListItemElement), "//li" },
			{ typeof(ComboBoxElement), "//input[@role='combobox']" },
			{ typeof(KendoUiComboboxElement), "//span[contains(@class, 'k-combobox')]/input" },
			{ typeof(KendoUiDropdownElement), "//span[contains(@class, 'k-combobox')]//select" },
			{ typeof(KendoNumericTextBoxElement), "//input[@data-role='numerictextbox']" }
		};
	}
}