namespace AutomationFramework.Core.Controls.Interfaces
{
	public interface ILabel : IHtmlElement, ITextContent, IClickable
	{
		void WaitForTextToBePresent(string elementText, int timeOut = 20);
    }
}
