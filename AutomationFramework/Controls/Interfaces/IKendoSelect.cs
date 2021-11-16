using System.Collections.Generic;

namespace AutomationFramework.Core.Controls.Interfaces
{
	public interface IKendoSelect
	{
		bool IsOpened { get; }

		bool IsLoaded { get; }

		IEnumerable<string> GetOptions();

		void SelectOptionByText(string textValue);

		void SelectOptionByPartialMatch(string textValue);

		void WaitUntilContentIsLoaded();
	}
}
