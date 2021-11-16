using System;

namespace AutomationFramework.Core.Controls.Interfaces
{
	public interface IDialogWithFooter : IDialog
	{
		void ClickButton(Enum button);

		IButton GetButton(Enum buttonName);
	}
}