using System;
using System.Collections.Generic;

using AutomationFramework.Core.Constants;
using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Engine;
using AutomationFramework.Core.Enums;
using AutomationFramework.Core.Extensions;
using AutomationFramework.Core.Utils;
using AutomationFramework.Core.Utils.Log;

using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

using By = AutomationFramework.Core.Engine.By;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace AutomationFramework.Core.Controls
{
	/// <summary>
	/// Base class for all custom HTML controls
	/// </summary>
	public class UiElement : IHtmlElement, IClickable
	{
		protected readonly IWebDriver Driver;

		protected readonly ElementSearchService ElementSearchService;

		protected readonly ILogger Log = Log4NetLogger.Instance;

		private readonly IWebElement _webElement;

		public UiElement(IWebElement webElement)
		{
			Driver = Browser.Instance;
			_webElement = webElement;
			ElementSearchService = new ElementSearchService();
		}

		/// <summary>
		/// Get background color CSS value
		/// </summary>
		public string BackgroundColor => _webElement.GetCssValue("background");

		public string Class => _webElement.GetAttribute("class");

		/// <summary>
		/// Get color CSS value
		/// </summary>
		public string Color => _webElement.GetCssValue("color");

		public string CssClass => _webElement.GetAttribute("className");

		public string Id => _webElement.GetAttribute("id");

		/// <summary>
		/// Inner HTML content of element
		/// </summary>
		public string InnerHtml => _webElement.GetAttribute("innerHTML");

		/// <summary>
		/// returns bool value if the content is editable
		/// </summary>
		public bool IsContentEditable => Convert.ToBoolean(Browser.InvokeScript("arguments[0].IsContentEditable;", _webElement));

		/// <summary>
		/// Is element currently active on the webpage
		/// </summary>
		public bool IsFocused => Browser.Instance.SwitchTo().ActiveElement().Location == _webElement.Location;

		/// <summary>
		/// Is element displayed on the current page
		/// </summary>
		public virtual bool IsVisible => _webElement.Displayed;

		/// <summary>
		/// Outer HTML content of element
		/// </summary>
		public string OuterHtml => _webElement.GetAttribute("outerHTML");

		public string PlaceholderAttribute => _webElement.GetAttribute("placeholder");

		public string TagName => _webElement.TagName;

		/// <summary>
		/// Click on the element
		/// </summary>
		/// <param name="actionType">Type of click action</param>
		/// <param name="mouseClickType">Type of mouse click, valid only for ActionType.Mouse</param>
		public virtual void Click(ActionType actionType = ActionType.Default, MouseClickType mouseClickType = MouseClickType.LeftClick)
		{
			WaitToBeClickable();

			switch (actionType)
			{
				case ActionType.Mouse:
					{
						MouseClick(mouseClickType);
						break;
					}

				case ActionType.JavaScript:
					{
						Browser.InvokeScript("arguments[0].click();", _webElement);

						Wait.ForPageReadyStateToComplete();
						Wait.ForAngularToComplete();
						break;
					}

				case ActionType.Default:
					{
						_webElement.Click();
						break;
					}
			}
		}

		public TElement FindElement<TElement>(By by)
			where TElement : class, IHtmlElement =>
			ElementSearchService.FindElement<TElement>(_webElement, by);

		public IEnumerable<TElement> FindElements<TElement>(By by)
			where TElement : class, IHtmlElement =>
			ElementSearchService.FindElements<TElement>(_webElement, by);

		/// <summary>
		/// Get the specified attribute from element
		/// </summary>
		/// <param name="name">Attribute name</param>
		/// <returns>Attribute</returns>
		public string GetAttribute(string name) => _webElement.GetAttribute(name);

		/// <summary>
		/// Get the specified attribute from element
		/// </summary>
		/// <param name="valueName">Attribute name</param>
		/// <returns>CSS Value</returns>
		public string GetCssValue(string valueName) => _webElement.GetCssValue(valueName);

		public T GetUnderlyingElement<T>()
			where T : UiElement, IHtmlElement
		{
			try
			{
				IWebElement element = _webElement.FindElement(OpenQA.Selenium.By.XPath("." + HtmlElementXPath.XPath[typeof(T)]));
				return element.As<T>();
			}
			catch (KeyNotFoundException)
			{
				string message = $"{typeof(T)} key is not found in HtmlElementXPath.XPath dictionary. Add it before using in code";
				Log.Error(message);
				throw new KeyNotFoundException(message);
			}
		}

		public List<T> GetUnderlyingElements<T>()
			where T : UiElement, IHtmlElement
		{
			try
			{
				var webElements = _webElement.FindElements(OpenQA.Selenium.By.XPath("." + HtmlElementXPath.XPath[typeof(T)]));
				var customElements = new List<T>();

				foreach (IWebElement element in webElements)
				{
					customElements.Add(element.As<T>());
				}

				return customElements;
			}
			catch (KeyNotFoundException)
			{
				Log.Error($"{typeof(T)} key is not found in HtmlElementXPath.XPath dictionary. Add it before using in code");
				throw;
			}
		}

		/// <summary>
		/// Get Id from WrappedElement
		/// </summary>
		/// <returns>
		/// UU ID
		/// </returns>
		public Guid GetUuId()
		{
			var element = (RemoteWebElement)_webElement.GetType().GetProperty("WrappedElement").GetValue(_webElement);
			var auxiliaryLocator = element.Coordinates.AuxiliaryLocator.ToString();
			return new Guid(auxiliaryLocator);
		}

		public void Hover()
		{
			var action = new Actions(Driver);
			action.MoveToElement(_webElement).Perform();
		}

		/// <summary>
		/// Invoke JavaScript on given element
		/// </summary>
		/// <param name="script">Script body</param>
		/// <returns>Object</returns>
		public object InvokeJavaScript(string script, params object[] args) => Browser.InvokeScript(script, _webElement);

		/// <summary>
		/// Remove focus from active element
		/// </summary>
		public void RemoveFocus() => Browser.InvokeScript("return document.activeElement.blur();");

		/// <summary>
		/// Scroll to the web element
		/// </summary>
		public void ScrollTo() => InvokeJavaScript("arguments[0].scrollIntoView(true);");

		/// <summary>
		/// Scroll the grid using keyboard keys
		/// </summary>
		/// <param name="keys">Keys to scroll with</param>
		public void ScrollUsingKeyboard(string keys)
		{
			var actionBuilder = new Actions(Browser.Instance);
			actionBuilder.MoveToElement(_webElement);
			actionBuilder.Click();
			actionBuilder.SendKeys(keys);
			actionBuilder.Build().Perform();
		}

		/// <summary>
		/// Focus on element invoking .focus() javaScript call
		/// </summary>
		public void SetFocus() => Browser.InvokeScript("arguments[0].focus();", _webElement);

		/// <summary>
		/// Wait for element to be clickable
		/// </summary>
		/// <param name="timeOut">Timeout, seconds</param>
		public void WaitToBeClickable(int timeOut = 20)
		{
			var wait = new WebDriverWait(Browser.Instance, TimeSpan.FromSeconds(timeOut));
			wait.Until(ExpectedConditions.ElementToBeClickable(_webElement));
		}

		public void WaitToBeVisible()
		{
			Wait.Until(() => IsVisible);
		}

		/// <summary>
		/// Wait until staleness of element
		/// </summary>
		/// <param name="timeOut">Timeout, seconds</param>
		public void WaitUntilStaleness(int timeOut = 20)
		{
			var wait = new WebDriverWait(Browser.Instance, TimeSpan.FromSeconds(timeOut));
			wait.Until(ExpectedConditions.StalenessOf(_webElement));
		}

		/// <summary>
		/// Click on element using Advanced Actions API
		/// </summary>
		/// <param name="clickTypeType">Mouse click type</param>
		private void MouseClick(MouseClickType clickTypeType)
		{
			var builder = new Actions(Driver);

			switch (clickTypeType)
			{
				case MouseClickType.LeftClick:
					{
						builder.MoveToElement(_webElement).Click().Build().Perform();
						break;
					}

				case MouseClickType.LeftDoubleClick:
					{
						_webElement.SendKeys(string.Empty);
						builder.MoveToElement(_webElement).DoubleClick().Build().Perform();
						break;
					}

				case MouseClickType.RightClick:
					{
						builder.MoveToElement(_webElement).ContextClick().Build().Perform();
						break;
					}

				default:
					throw new ArgumentOutOfRangeException(nameof(clickTypeType), clickTypeType, null);
			}
		}
	}
}