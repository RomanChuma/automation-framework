using AutomationFramework.Core.Enums;

namespace AutomationFramework.Core.Controls.Interfaces
{
	public interface IClickable
	{
		void Click(ActionType actionType = ActionType.Default, MouseClickType mouseClickType = MouseClickType.LeftClick);

		void WaitToBeClickable(int timeOut = 20);
	}
}
