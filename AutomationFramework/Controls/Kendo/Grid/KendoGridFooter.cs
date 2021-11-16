using System;
using System.Collections.Generic;
using System.Linq;

using AutomationFramework.Core.Controls.Grid.Pager;
using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Engine;
using AutomationFramework.Core.Extensions;

using OpenQA.Selenium;

using By = AutomationFramework.Core.Engine.By;

namespace AutomationFramework.Core.Controls.Kendo.Grid
{
	public class KendoGridFooter : UiElement, IPageNumbering
	{
		private readonly UiElement _pager;

		public KendoGridFooter(IWebElement webElement)
			: base(webElement)
		{
			_pager = webElement.As<UiElement>();
		}

		public IButton BtnActivePage => Browser.FindElement<ButtonElement>(By.XPath("//li[@class='k-current-page']/span"));

		public IButton BtnNext =>
			Browser.FindElement<KendoUiButtonElement>(By.XPath("//span[@class='k-icon k-i-arrow-60-right']/parent::a"));

		public IButton BtnStart =>
			Browser.FindElements<KendoUiButtonElement>(By.XPath("//a[contains(@class, 'k-link k-pager-nav k-pager-first')]"))
				.FirstOrDefault();

		public KendoUiDropdownElement PerPageSelector =>
			_pager.FindElement<KendoUiDropdownElement>(By.XPath(".//span[@class='k-widget k-dropdown']//select"));

		private IButton BtnEnd =>
			Browser.FindElement<KendoUiButtonElement>(By.XPath("//a[contains(@class, 'k-link k-pager-nav k-pager-last')]"));

		private IButton BtnLastPage =>
			Browser.FindElements<ButtonElement>(By.XPath("//span[@ng-if='pager.isLastPageShown']")).FirstOrDefault();

		private IDiv PageNumbering => Browser.FindElement<DivElement>(By.XPath("//div[@class='k-pager-numbers-wrap']"));

		/// <summary>
		///  Get all available buttons
		/// </summary>
		/// <returns> List of buttons available to select.</returns>
		public List<ButtonElement> GetPageButtons()
		{
			var pages = PageNumbering.FindElements<ButtonElement>(By.XPath("//span")).ToList();
			return pages;
		}

		public void GoToEnd()
		{
			if (!BtnEnd.IsEnabled)
			{
				Log.Warn("Grid is shown on the 1 page, skipping");
				return;
			}

			BtnEnd.Click();
		}

		public void GoToNextPage()
		{
			if (BtnNext != null && BtnNext.IsEnabled)
			{
				BtnNext.Click();
			}
			else
			{
				var message = "Can't go to next grid page";
				Log.Error(message);
				throw new InvalidOperationException(message);
			}
		}

		public void GoToStart()
		{
			try
			{
				BtnStart.Click();
			}
			catch (Exception e)
			{
				var message = "Can't go to grid start";
				Log.Error(message, e);
				throw new InvalidOperationException(message);
			}
		}

		/// <summary>
		/// Is page indicator of given page number active
		/// </summary>
		/// <param name="pageNumber">
		/// The page Number.
		/// </param>
		/// <returns>
		/// Boolean is page selected
		/// </returns>
		public bool IsPageSelected(int pageNumber)
		{
			bool isSelected = GetPageButtons()[pageNumber].Class.Contains("page active");
			return isSelected;
		}

		/// <summary>
		/// Select last page in the grid footer
		/// </summary>
		public void SelectLastPage()
		{
			BtnLastPage.Click();
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