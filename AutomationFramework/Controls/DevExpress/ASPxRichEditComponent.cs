using System;
using System.Linq;

using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Engine;
using AutomationFramework.Core.Utils;

using OpenQA.Selenium;

using By = AutomationFramework.Core.Engine.By;

namespace AutomationFramework.Core.Controls.DevExpress
{
	/// <summary>
	/// Implementation of DevExpress ASP.NET Rich Text Editor
	/// See https://docs.devexpress.com/AspNet/DevExpress.Web.ASPxRichEdit.ASPxRichEdit
	/// </summary>
	public class AspXRichEditComponent : UiElement, IASPxRichEdit
	{
		private readonly IWebElement _aspxRichEditComponent;

		public AspXRichEditComponent(IWebElement webElement)
			: base(webElement)
		{
			_aspxRichEditComponent = webElement;
			SetClientInstanceName();
		}

		public ASPxRteContextMenu ActionMenu =>
			Browser.FindElements<ASPxRteContextMenu>(By.XPath("//ul[@class='dx dxm-gutter' and @role='menubar']"))
				.FirstOrDefault(x => x.IsVisible);

		public string CallbackUrl
		{
			get
			{
				string callbackUrlScript = $"return {ClientInstanceName}.callbackUrl";
				var callbackUrl = Convert.ToString(Browser.InvokeScript(callbackUrlScript));
				return callbackUrl;
			}
		}

		/// <summary>
		/// Gets active sub-document identifier.
		/// See https://docs.devexpress.com/AspNet/js-SubDocument.id
		/// </summary>
		/// <returns>Int</returns>
		public int SubDocumentId
		{
			get
			{
				string documentIdScript = $"return {ClientInstanceName}.document.activeSubDocument.id";
				var documentId = Convert.ToInt32(Browser.InvokeScript(documentIdScript));
				return documentId;
			}
		}

		public IUnorderedList SubMenu =>
			Browser.FindElement<UnorderedListElement>(By.XPath("//ul[@class='dx dxm-gutter' and @role='menu']"));

		/// <summary>
		/// Gets the whole text of the main sub-document
		/// </summary>
		public string WholeSubdocumentText
		{
			get
			{
				string getWholeTextCommandScript = $"return {ClientInstanceName}.document.mainSubDocument.text;";
				return Convert.ToString(Browser.InvokeScript(getWholeTextCommandScript));
			}
		}

		/// <summary>
		/// Gets or sets client instance name
		/// </summary>
		private string ClientInstanceName { get; set; }

		/// <summary>
		/// Executes a command to delete the text in a selected range.
		/// </summary>
		public void Delete()
		{
			string deleteCommandScript = $@"return {ClientInstanceName}.commands.delete.execute();";
			Browser.InvokeScript(deleteCommandScript);
		}

		public void ExpandConditionSubmenu()
		{
			var menuItems = ActionMenu.FindElement<ListItemElement>(By.XPath(".//li[@class='dxm-item dxm-subMenu']"));
			AnchorElement insertConditionItem = menuItems.GetUnderlyingElement<AnchorElement>();
			insertConditionItem.Hover();
			insertConditionItem.Click();
		}

		/// <summary>
		/// Executes command to activate the page header and begin editing.
		/// </summary>
		public void InsertHeader()
		{
			string insertHeaderScript = $@"return {ClientInstanceName}.commands.insertHeader.execute();";
			Browser.InvokeScript(insertHeaderScript);
		}

		/// <summary>
		/// Executes command to insert an inline picture stored by the specified web address.
		/// </summary>
		/// <param name="path">Path to photo</param>
		public void InsertPicture(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentException("Value cannot be null or empty.", nameof(path));
			}

			string insertPictureScript = $@"{ClientInstanceName}.commands.insertPicture.execute('{path}')";
			Browser.InvokeScript(insertPictureScript);
		}

		/// <summary>
		/// Insert Text into the RTE
		/// </summary>
		/// <param name="text">Text to insert</param>
		/// <param name="insertPosition">Insert position </param>
		/// <param name="subDocumentId">An integer value specifying the sub-document</param>
		public void InsertText(string text, int insertPosition, int subDocumentId)
		{
			if (insertPosition < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(insertPosition));
			}

			if (subDocumentId < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(subDocumentId));
			}

			string insertTextScript =
				$@"return {ClientInstanceName}.commands.insertText.execute('{text}', {insertPosition}, {subDocumentId})";
			var actionWasSuccessful = Convert.ToBoolean(Browser.InvokeScript(insertTextScript));

			ValidateResult(actionWasSuccessful, "Insert text");
		}

		/// <summary>
		/// Insert Text into the RTE
		/// </summary>
		/// <param name="text">Text to insert</param>
		/// <param name="insertPosition">Insert position </param>
		public void InsertText(string text, int insertPosition)
		{
			if (insertPosition < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(insertPosition));
			}

			string insertTextScript = $@"return {ClientInstanceName}.commands.insertText.execute('{text}', {insertPosition})";
			var actionWasSuccessful = Convert.ToBoolean(Browser.InvokeScript(insertTextScript));

			ValidateResult(actionWasSuccessful, "Insert text");
		}

		/// <summary>
		/// Insert Text into the RTE at current cursor position
		/// </summary>
		/// <param name="text">Text to insert</param>
		public void InsertText(string text)
		{
			string insertTextScript = $@"return {ClientInstanceName}.commands.insertText.execute('{text}')";
			var actionWasSuccessful = Convert.ToBoolean(Browser.InvokeScript(insertTextScript));

			ValidateResult(actionWasSuccessful, "Insert text");
		}

		/// <summary>
		/// Selects the editor's entire content.
		/// </summary>
		public void SelectAll()
		{
			string selectCommandScript = $@"return {ClientInstanceName}.selection.selectAll();";
			Browser.InvokeScript(selectCommandScript);
		}

		public void WaitForLoadingMaskToDisappear()
		{
			var waitScript = "return document.getElementById('clientLetterEditor_LP').style.display == 'none'";
			Wait.Until(() => Convert.ToBoolean(Browser.InvokeScript(waitScript)));
		}

		private void SetClientInstanceName()
		{
			// If the ClientInstanceName property is not specified for a control,
			// the control's client identifier is generated automatically and equals the value of the control's ID﻿ property.
			ClientInstanceName = _aspxRichEditComponent.GetAttribute("id");
		}

		private void ValidateResult(bool actionWasSuccessful, string actionName)
		{
			if (actionWasSuccessful)
			{
				return;
			}

			throw new InvalidOperationException($"{actionName} was not successful");
		}
	}
}