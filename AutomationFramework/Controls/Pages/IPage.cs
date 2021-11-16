namespace AutomationFramework.Core.Controls.Pages
{
	public interface IPage
	{
		string Title { get; }

		string PageUrl { get; }

		bool IsOpened { get; }

		void WaitToBeOpened();
	}
}