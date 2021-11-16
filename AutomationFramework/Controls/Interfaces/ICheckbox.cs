using System.Collections.Generic;

using AutomationFramework.Core.Enums;

namespace AutomationFramework.Core.Controls.Interfaces
{
	public interface ICheckbox : IHtmlElement, IEnabled
	{
		bool IsChecked { get; }

		List<ElementState> State { get; }

		void Check(ActionType actionType = ActionType.Default);

		void Uncheck(ActionType actionType = ActionType.Default);
	}
}
