namespace AutomationFramework.Core.Controls.Interfaces
{
	public interface IDropzone : IHtmlElement, IClickable
	{
		void DropFile(string filePath);
	}
}
