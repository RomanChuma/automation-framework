using System;

using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Engine;
using AutomationFramework.Core.Enums;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;

namespace AutomationFramework.Core.Controls
{
	/// <summary>
	/// Concrete implementation of Combo Box element with autocomplete
	/// </summary>
	public class ComboBoxElement : UiElement, IComboBox
	{
		private readonly IWebElement _comboBox;

		public ComboBoxElement(IWebElement webElement)
			: base(webElement)
		{
			_comboBox = webElement;
		}

		public bool IsEnabled => _comboBox.Enabled;

		/// <summary>
		/// Gets value indicating the 'value' HTML attribute
		/// </summary>
		public string SelectedOption => _comboBox.GetAttribute("value");

		public string Text => _comboBox.Text;

		public string Value => _comboBox.GetAttribute("value");

		public void Clear(ActionType clearType = ActionType.Default)
		{
			switch (clearType)
			{
				case ActionType.Default:
					_comboBox.Clear();
					break;
				case ActionType.Keyboard:
					_comboBox.SendKeys(Keys.Control + "a");
					_comboBox.SendKeys(Keys.Delete);
					break;
				default:
					throw new NotImplementedException($"{clearType.ToString()} clear action is not implemented");
			}
		}

		public void Close() => _comboBox.Click();

		public void Open() => _comboBox.Click();

		/// <summary>
		/// Set text to comboBox element
		/// </summary>
		/// <param name="text">
		/// Text to set
		/// </param>
		/// <param name="setTextType">
		/// Type of text set
		/// </param>
		/// <param name="clearField">
		/// The clear Field.
		/// </param>
		public void SetText(string text, SetTextType setTextType = SetTextType.Default, bool clearField = true)
		{
			switch (setTextType)
			{
				case SetTextType.Default:

					if (clearField)
					{
						Clear(ActionType.Keyboard);
					}

					_comboBox.SendKeys(text);
					_comboBox.SendKeys(Keys.Enter);

					RemoveFocus();
					break;

				case SetTextType.JavaScript:
					Browser.Instance.ExecuteJavaScript("arguments[0].value = arguments[1]; ", _comboBox, text);
					RemoveFocus();
					break;
			}
		}
	}
}