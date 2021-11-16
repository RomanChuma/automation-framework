using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Engine;
using AutomationFramework.Core.Enums;
using AutomationFramework.Core.Utils;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;

namespace AutomationFramework.Core.Controls.Kendo
{
	/// <summary>
	/// Kendo UI ComboBox component for Angular
	/// </summary>
	public class KendoUiComboboxElement : KendoSelect, IKendoCombobox
	{
		public KendoUiComboboxElement(IWebElement webElement)
			: base(webElement)
		{
		}

		public override bool IsVisible => ParentComponent.IsVisible;

		public override int OptionsCount
		{
			get
			{
				var optionsCount = (long)Browser.InvokeScript(
					$"return $(arguments[0]).data('{SelectType}').ul.children().toArray().length;",
					KendoSelectElement);

				return Convert.ToInt32(optionsCount);
			}
		}

		/// <summary>Gets the selector.</summary>
		/// <value>The selector.</value>
		protected override string SelectType => "kendoComboBox";

		public void Clear(ActionType clearType = ActionType.Default)
		{
			switch (clearType)
			{
				case ActionType.Default:
					VisibleInput.Clear();
					break;
				case ActionType.Keyboard:
					VisibleInput.SendKeys(Keys.Control + "a");
					VisibleInput.SendKeys(Keys.Delete);
					break;
				default:
					throw new NotImplementedException($"{clearType.ToString()} clear action is not implemented");
			}
		}

		public override IEnumerable<string> GetOptions()
		{
			Wait.Until(() => OptionsCount > 0);

			var elements = (ReadOnlyCollection<IWebElement>)Browser.InvokeScript(
				$"return $(arguments[0]).data('{SelectType}').ul.children().toArray();",
				KendoSelectElement);
			var options = elements.Select(element => (string)Browser.InvokeScript("return arguments[0].textContent", element));
			return options;
		}

		public void SetText(string text, SetTextType setTextType = SetTextType.Default, bool clearField = true)
		{
			switch (setTextType)
			{
				case SetTextType.Default:

					if (clearField)
					{
						VisibleInput.Click();
						Clear(ActionType.Keyboard);
					}

					VisibleInput.SendKeys(text);
					VisibleInput.SendKeys(Keys.Enter);
					RemoveFocus();
					break;

				case SetTextType.JavaScript:
					string scriptToSetTextViaJQuery =
						$"$(arguments[0]).data('{SelectType}').select(function(dataItem) {{return dataItem.text === '{text}';}});";
					Browser.Instance.ExecuteJavaScript(scriptToSetTextViaJQuery, KendoSelectElement);
					break;
			}
		}
	}
}