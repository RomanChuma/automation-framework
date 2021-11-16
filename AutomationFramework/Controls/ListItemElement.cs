using OpenQA.Selenium;

namespace AutomationFramework.Core.Controls
{
    public class ListItemElement : UiElement
    {
        private readonly IWebElement _webElement;
        public ListItemElement(IWebElement webElement) : base(webElement)
        {
            _webElement = webElement;
        }

        public string Text => _webElement.Text;

        public bool IsSelected => _webElement.Selected;
    }
}