using System.Collections.Generic;

using AutomationFramework.Core.Engine;

namespace AutomationFramework.Core.Controls.Interfaces
{
	public interface IHtmlElement : IVisible, IElementFinder
	{
		bool IsFocused { get; }

		string OuterHtml { get; }

		string InnerHtml { get; }

		string CssClass { get; }

		string BackgroundColor { get; }

		string Color { get; }

		string Class { get; }

		List<T> GetUnderlyingElements<T>() where T : UiElement, IHtmlElement;

		T GetUnderlyingElement<T>() where T : UiElement, IHtmlElement;

		void SetFocus();

		void RemoveFocus();

		void Hover();

		void ScrollTo();

		void ScrollUsingKeyboard(string keys);

		string GetAttribute(string name);

		string GetCssValue(string valueName);

		object InvokeJavaScript(string script, params object[] args);

        void WaitUntilStaleness(int timeOut = 20);
    }
}