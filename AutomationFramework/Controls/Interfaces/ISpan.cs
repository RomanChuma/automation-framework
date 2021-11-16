namespace AutomationFramework.Core.Controls.Interfaces
{
	public interface ISpan : IHtmlElement, ITextContent, IClickable
	{
		void WaitForTextToBePresent(string elementText, int timeOut = 20);
	}
}