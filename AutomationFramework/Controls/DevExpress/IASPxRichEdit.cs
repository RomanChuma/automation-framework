using AutomationFramework.Core.Controls.Interfaces;

namespace AutomationFramework.Core.Controls.DevExpress
{
	public interface IASPxRichEdit
	{
		ASPxRteContextMenu ActionMenu { get; }

		string CallbackUrl { get; }

		int SubDocumentId { get; }

		IUnorderedList SubMenu { get; }

		string WholeSubdocumentText { get; }

		void Delete();

		void ExpandConditionSubmenu();

		void InsertHeader();

		void InsertPicture(string path);

		void InsertText(string text, int insertPosition, int subDocumentId);

		void InsertText(string text, int insertPosition);

		void InsertText(string text);

		void SelectAll();
	}
}