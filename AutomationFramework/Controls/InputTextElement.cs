using System;

using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Engine;
using AutomationFramework.Core.Enums;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;

namespace AutomationFramework.Core.Controls
{
	/// <summary>
	/// Implementation of input text element
	/// </summary>
	public class InputTextElement : UiElement, ITextBox
	{
		private readonly IWebElement _textElement;

		public InputTextElement(IWebElement webElement) : base(webElement)
		{
			_textElement = webElement;
		}

		/// <summary>
		/// Text box content
		/// </summary>
		public string Text => _textElement.Text;

		/// <summary>
		/// Is element enabled
		/// </summary>
		public bool IsEnabled => _textElement.Enabled;

		public string Value => _textElement.GetAttribute("value");

		/// <summary>
		/// Set text into text field
		/// </summary>
		/// <param name="text">Text to set</param>
		/// <param name="setTextType">Type of setting the text, <see cref="SetTextType"/></param>
		/// <param name="clearField">Clear field before setting the text</param>
		public void SetText(string text, SetTextType setTextType = SetTextType.Default, bool clearField = true)
		{
			switch (setTextType)
			{
				case SetTextType.Default:
					if (clearField)
					{
						Clear();
					}

					_textElement.SendKeys(text);
					break;

				case SetTextType.JavaScript:
					Browser.Instance.ExecuteJavaScript("arguments[0].value = arguments[1]; ", _textElement, text);
					RemoveFocus();
					break;
			}
		}

		/// <summary>
		/// Clear field
		/// </summary>
		/// <param name="actionType">
		/// The action Type.
		/// </param>
		public void Clear(ActionType actionType = ActionType.Default)
		{
			switch (actionType)
			{
				case ActionType.Default:
					_textElement.Clear();
					break;
				case ActionType.Keyboard:
					_textElement.SendKeys(Keys.Control + "a");
					_textElement.SendKeys(Keys.Delete);
					break;
				default:
					throw new NotImplementedException($"{actionType} clear action is not implemented");
			}
		}
	}
}