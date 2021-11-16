namespace AutomationFramework.Core.Controls.Interfaces
{
	public interface IVisible
	{
		bool IsVisible { get; }

		void WaitToBeVisible();
	}
}