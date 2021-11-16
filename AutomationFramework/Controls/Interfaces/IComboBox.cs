namespace AutomationFramework.Core.Controls.Interfaces
{
	public interface IComboBox : ITextBox
	{
		string SelectedOption { get; }

		void Open();

		void Close();
	}
}
