namespace AutomationFramework.Core.Controls.Interfaces
{
	public interface IButton : IHtmlElement, IClickable, IEnabled
	{
		string Text { get; }
    }
}