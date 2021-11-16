using System.Collections.Generic;

using AutomationFramework.Core.Controls.Interfaces;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutomationFramework.Core.Controls
{
	/// <summary>
	/// Concrete implementation of dropdown element
	/// </summary>
	public class DropdownElement : UiElement, IDropDown
	{
		private readonly SelectElement _dropDown;

		public DropdownElement(IWebElement webElement)
			: base(webElement)
		{
			_dropDown = new SelectElement(webElement);
		}

		public bool IsEnabled => _dropDown.SelectedOption.Enabled;

		/// <summary>
		/// Get dropdown list items count
		/// </summary>
		public int OptionsCount => _dropDown.Options.Count;

		public List<string> OptionsText
		{
			get
			{
				var optionsElements = _dropDown.Options;
				var optionsText = new List<string>();

				foreach (IWebElement option in optionsElements)
				{
					optionsText.Add(option.Text);
				}

				return optionsText;
			}
		}

		/// <summary>
		/// Gets selected option text within the dropdown
		/// </summary>
		public string SelectedOption
		{
			get
			{
				Log.Trace($"Getting selected value from dropdown: {_dropDown.SelectedOption.Text}");
				return _dropDown.SelectedOption.Text;
			}
		}

		public void SelectOptionByIndex(int index)
		{
			_dropDown.SelectByIndex(index);
		}

		public void SelectOptionByPartialMatch(string textValue)
		{
			var options = OptionsText;

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

		/// <summary>
		/// Select dropdown item by it's text
		/// </summary>
		/// <param name="optionText">Item text</param>
		public void SelectOptionByText(string optionText)
		{
			Log.Trace($"Selecting '{optionText}' item from dropdown");
			_dropDown.SelectByText(optionText);
		}

		/// <summary>
		/// Select dropdown item by it's value
		/// </summary>
		/// <param name="itemName">Item value</param>
		public void SelectOptionByValue(string itemName)
		{
			_dropDown.SelectByValue(itemName);
		}
	}
}