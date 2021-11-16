using System.Collections.Generic;
using System.Linq;

using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Engine;
using AutomationFramework.Core.Extensions;

using OpenQA.Selenium;

using By = AutomationFramework.Core.Engine.By;

namespace AutomationFramework.Core.Controls.Kendo
{
	public class KendoUiDropdownElement : KendoSelect, IDropDown
	{
		private readonly UiElement _dropDown;

		public KendoUiDropdownElement(IWebElement webElement)
			: base(webElement)
		{
			_dropDown = webElement.As<UiElement>();
		}

		public override int OptionsCount
		{
			get
			{
				var optionsCount = (int)Browser.InvokeScript(
					$"return $(arguments[0]).data('{SelectType}').dataSource.options.data.length;",
					KendoSelectElement);

				return optionsCount;
			}
		}

		public List<string> OptionsText => GetOptions().ToList();

		/// <summary>Gets the selector.</summary>
		/// <value>The selector.</value>
		protected override string SelectType => "kendoDropDownList";

		public override IEnumerable<string> GetOptions()
		{
			var options = _dropDown.FindElements<UiElement>(By.XPath(".//option"));
			var text = options.Select(x => x.InnerHtml).ToList();
			return text;
		}

		public void SelectOptionByValue(string itemName)
		{
			SelectOptionByText(itemName);
		}
	}
}