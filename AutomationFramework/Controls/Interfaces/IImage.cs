namespace AutomationFramework.Core.Controls.Interfaces
{
	public interface IImage : IHtmlElement, IClickable, ITextContent
	{
		string Source { get; }
	}
}