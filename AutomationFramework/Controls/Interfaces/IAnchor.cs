namespace AutomationFramework.Core.Controls.Interfaces
{
	/// <summary>
	/// Interface for Anchor element
	/// </summary>
	public interface IAnchor : IHtmlElement, ITextContent, IClickable, IEnabled
	{
		string Url { get; }

		string Title { get; }
	}
}