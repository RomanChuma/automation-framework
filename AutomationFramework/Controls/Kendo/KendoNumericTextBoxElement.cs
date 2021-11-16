using System;

using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Engine;
using AutomationFramework.Core.Enums;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;

namespace AutomationFramework.Core.Controls.Kendo
{
	/// <summary>
	/// Kendo UI Numeric Text Box for Angular
	/// </summary>
	public class KendoNumericTextBoxElement : UiElement, ITextBox
	{
		private readonly IWebElement _textElement;

		public KendoNumericTextBoxElement(IWebElement webElement)
			: base(webElement)
		{
			_textElement = webElement;
		}

		/// <summary>
		/// Is element enabled
		/// </summary>
		public bool IsEnabled => _textElement.Enabled;

		/// <summary>
		/// Text box content
		/// </summary>
		public string Text => _textElement.Text;

		public string Value => _textElement.GetAttribute("value");

		public void Clear(ActionType clearType = ActionType.Default)
		{
			switch (clearType)
			{
				case ActionType.Default:
					_textElement.Clear();
					break;
				case ActionType.Keyboard:
					_textElement.SendKeys(Keys.Control + "a");
					_textElement.SendKeys(Keys.Delete);
					break;
				default:
					throw new NotImplementedException($"{clearType} clear action is not implemented");
			}
		}

		/// <summary>
		/// Set text into text field
		/// </summary>
		/// <param name="text">Text to set</param>
		/// <param name="setTextType">Type of setting the text, <see cref="SetTextType"/></param>
		/// <param name="clearField">Clear field before setting the text</param>
		public void SetText(string text, SetTextType setTextType = SetTextType.JavaScript, bool clearField = true)
		{
			if (clearField)
			{
				_textElement.Click();
				Clear(ActionType.Keyboard);
			}

			Browser.Instance.ExecuteJavaScript("arguments[0].value = arguments[1]; ", _textElement, text);
			RemoveFocus();
		}
	}
}