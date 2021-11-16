using AutomationFramework.Core.Engine;
using AutomationFramework.Core.Utils;

namespace AutomationFramework.Core.Controls.Pages
{
	public abstract class BasePage : IPage
	{
		public string Title => Browser.Title;

		public virtual string PageUrl { get; }

		public bool IsOpened
		{
			get
			{
				string currentOpenedUrl = Browser.CurrentUrl;
				bool isPageOpened = currentOpenedUrl.Contains(PageUrl);
				return isPageOpened;
			}
		}

		public void WaitToBeOpened()
		{
			Wait.ForPartUrlToBeOpened(PageUrl);
		}
	}
}