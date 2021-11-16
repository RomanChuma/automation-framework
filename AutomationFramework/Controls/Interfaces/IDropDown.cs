using System.Collections.Generic;

namespace AutomationFramework.Core.Controls.Interfaces
{
	public interface IDropDown : IHtmlElement, IClickable, IEnabled
	{
		int OptionsCount { get; }

		List<string> OptionsText { get; }

		string SelectedOption { get; }

		void SelectOptionByPartialMatch(string text);

		void SelectOptionByText(string optionText);

		void SelectOptionByIndex(int index);

		void SelectOptionByValue(string itemName);
	}
}