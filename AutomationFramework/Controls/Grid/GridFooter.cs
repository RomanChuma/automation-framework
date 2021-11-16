using System;
using System.Collections.Generic;
using System.Linq;

using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Engine;
using AutomationFramework.Core.Extensions;

using OpenQA.Selenium;

namespace AutomationFramework.Core.Controls.Grid
{
	public class GridFooter : UiElement
	{
		private readonly UiElement _pager;

		public GridFooter(IWebElement webElement)
			: base(webElement)
		{
			_pager = webElement.As<UiElement>();
		}

		public ISpan SpanPageLabel => Browser.FindElement<SpanElement>(Engine.By.CssSelector("span#pg_PageLabel"));

		public ITextBox InputPageNumber => Browser.FindElement<InputTextElement>(Engine.By.CssSelector("input#pageNumber"));

		public ISpan SpanRemainingPageText => Browser.FindElement<SpanElement>(Engine.By.XPath("//input[@id='pageNumber']/following-sibling::span[1]"));

		public string PageNavigationalText => $"{SpanPageLabel.Text} {InputPageNumber.Value} {SpanRemainingPageText.Text}";

		public ISpan SpanTotalItem => Browser.FindElement<SpanElement>(Engine.By.XPath("//span[@id='pg_TotalItems']"));

		public IAnchor LnkNext =>
			Browser.FindElement<AnchorElement>(Engine.By.XPath("//div[@class='pg_NumberBar']//a[@class='pg_text' and contains(text(),'›')]"));

		public IAnchor LnkEnd =>
			Browser.FindElement<AnchorElement>(Engine.By.XPath("//div[@class='pg_NumberBar']//a[@class='pg_text' and contains(text(),'»')]"));

		public IAnchor LnkStart =>
			Browser.FindElement<AnchorElement>(Engine.By.XPath("//div[@class='pg_NumberBar']//a[@class='pg_text' and contains(text(),'« ')]"));

		public IAnchor LnkPrevious =>
			Browser.FindElement<AnchorElement>(Engine.By.XPath("//div[@class='pg_NumberBar']//a[@class='pg_text' and contains(text(),'‹')]"));

		public IDropDown PerPageSelector =>
			_pager.FindElement<DropdownElement>(Engine.By.XPath(".//div[@class='pg_QuickPage']/select[@id='pagesize']"));

		public IDiv PageNumberingBar => Browser.FindElement<DivElement>(Engine.By.XPath("//div[@class='pg_NumberBar']"));

		public int TotalItems => int.Parse(SpanTotalItem.GetAttribute("total"));

		public List<ButtonElement> GetPageButtons()
		{
			var pages = PageNumberingBar.FindElements<ButtonElement>(Engine.By.XPath("//a")).ToList();
			return pages;
		}

		public void ClickEnd()
		{
			if (!LnkEnd.IsEnabled)
			{
				Log.Warn("Grid is shown on the 1 page, skipping");
				return;
			}

			LnkEnd.Click();
		}

		public void ClickNextPage()
		{
			if (LnkNext != null && LnkNext.IsEnabled)
			{
				LnkNext.Click();
			}
			else
			{
				var message = "Can't go to next grid page";
				Log.Error(message);
				throw new InvalidOperationException(message);
			}
		}

		public void ClickStartPage()
		{
			if (LnkStart != null && LnkStart.IsEnabled)
			{
				LnkStart.Click();
			}
			else
			{
				var message = "Can't go to start page";
				Log.Error(message);
				throw new InvalidOperationException(message);
			}
		}

		public void ClickPreviousPage()
		{
			if (LnkPrevious != null && LnkPrevious.IsEnabled)
			{
				LnkPrevious.Click();
			}
			else
			{
				var message = "Can't go to previous page";
				Log.Error(message);
				throw new InvalidOperationException(message);
			}
		}

		public void SelectPage(int pageNumber)
		{
			try
			{
				ButtonElement pageToSelect = GetPageButtons().First(p => p.Text == pageNumber.ToString());
				pageToSelect.Click();
			}
			catch (Exception e)
			{
				string message = $"Was not able to select grid page with number '{pageNumber}'";
				Log.Error(message, e);
				throw new InvalidOperationException(message);
			}
		}
	}
}
