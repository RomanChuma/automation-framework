using AutomationFramework.Core.Enums;

namespace AutomationFramework.Core.Controls.Interfaces
{
	public interface ITextBox : IHtmlElement, ITextContent, IClickable, IEnabled
	{
		string PlaceholderAttribute { get; }

		void SetText(string text, SetTextType setTextType = SetTextType.Default, bool clearField = true);

		void Clear(ActionType clearType = ActionType.Default);
	}
}
