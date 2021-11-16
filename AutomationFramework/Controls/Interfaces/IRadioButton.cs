using AutomationFramework.Core.Enums;

namespace AutomationFramework.Core.Controls.Interfaces
{
	public interface IRadioButton : IHtmlElement, IEnabled
	{
		bool IsSelected { get; }

		void Select(ActionType actionType = ActionType.Default);
	}
}
