using AutomationFramework.Core.Controls.Interfaces;

using OpenQA.Selenium;

namespace AutomationFramework.Core.Controls
{
    public class ImageElement : UiElement, IImage
    {
        private readonly IWebElement _webElement;

        public ImageElement(IWebElement webElement) : base(webElement)
        {
            _webElement = webElement;
        }

		/// <summary>
		/// URL of an image
		/// </summary>
		public string Source => _webElement.GetAttribute("src");

		/// <summary>
		/// Alternate text for an image
		/// </summary>
		public string Text => _webElement.GetAttribute("alt");

        public string Value => _webElement.GetAttribute("value");
    }
}
