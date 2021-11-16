using System;

using AutomationFramework.Core.Controls.Interfaces;
using AutomationFramework.Core.Enums;
using AutomationFramework.Core.Utils;

using OpenQA.Selenium;

namespace AutomationFramework.Core.Controls
{
	/// <summary>
	/// Concrete implementation of custom Anchor element
	/// </summary>
    public class AnchorElement : UiElement, IAnchor
    {
        private readonly IWebElement _anchor;

        public AnchorElement(IWebElement webElement) : base(webElement)
        {
            _anchor = webElement;
        }

		/// <summary>
		/// Gets value indicating is anchor element enabled
		/// </summary>
        public bool IsEnabled => _anchor.Enabled;

		/// <summary>
		/// Text content of Anchor element
		/// </summary>
		public string Text => _anchor.Text;

        public string Value => _anchor.GetAttribute("value");

        /// <summary>
		/// Value of 'href' attribute for the anchor
		/// </summary>
        public string Url => _anchor.GetAttribute("href");

        /// <summary>
        /// Value of 'Title' attribute for the anchor
        /// </summary>
        public string Title => _anchor.GetAttribute("title");

        /// <summary>
        /// Gets Anchor text font weight
        /// </summary>
        public FontWeight FontWeight
        {
            get
            {
                int weightValue = Convert.ToInt32(_anchor.GetCssValue("font-weight"));
                return HtmlStyleHelper.GetFontWeightNameFromItsValue(weightValue);
            }
        }
    }
}