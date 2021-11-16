using System;
using System.Globalization;

using AutomationFramework.Core.Utils;
using AutomationFramework.Core.Utils.Log;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;

namespace AutomationFramework.Core.Engine
{
	/// <summary>
	/// Class to override default Selenium methods in order to add additional logic
	/// </summary>
	public class EventFiringBrowser : EventFiringWebDriver
	{
		private static readonly ILogger Log = Log4NetLogger.Instance;

		/// <summary>
		/// Initializes a new instance of the <see cref="EventFiringBrowser"/> class.
		/// </summary>
		/// <param name="parentDriver">The parent driver.</param>
		public EventFiringBrowser(IWebDriver parentDriver) : base(parentDriver)
		{
		}

		/// <summary>
		/// Raises the <see cref="E:Navigated" /> event.
		/// </summary>
		/// <param name="e">The <see cref="WebDriverNavigationEventArgs"/> instance containing the event data.</param>
		protected override void OnNavigated(WebDriverNavigationEventArgs e)
		{
			Wait.ForPageReadyStateToComplete();
			Wait.ForAngularToComplete();

			base.OnNavigated(e);
		}

		/// <summary>
		/// Raises the <see cref="E:ElementClicking" /> event.
		/// </summary>
		/// <param name="e">The <see cref="WebElementEventArgs"/> instance containing the event data.</param>
		protected override void OnElementClicking(WebElementEventArgs e)
		{
			var elementStringRepresentation = GetElementStringRepresentation(e);
			Log.Trace($"Clicking on: '{elementStringRepresentation}' element");

			base.OnElementClicking(e);
		}

		/// <summary>
		/// Raises the <see cref="E:ElementClicked" /> event.
		/// </summary>
		/// <param name="e">The <see cref="WebElementEventArgs"/> instance containing the event data.</param>
		protected override void OnElementClicked(WebElementEventArgs e)
		{
			Wait.ForPageReadyStateToComplete();
			Wait.ForAngularToComplete();

			base.OnElementClicked(e);
		}

		/// <summary>
		/// Raises the <see cref="E:ElementValueChanged" /> event.
		/// </summary>
		/// <param name="e">The <see cref="WebElementEventArgs"/> instance containing the event data.</param>
		protected override void OnElementValueChanged(WebElementValueEventArgs e)
		{
			var elementStringRepresentation = GetElementStringRepresentation(e);
			Log.Trace($"On Element Value Changed: '{elementStringRepresentation}'");

			Wait.ForAngularToComplete();

			base.OnElementValueChanged(e);
		}

		/// <summary>
		/// Raises the <see cref="E:FindElement" /> event.
		/// </summary>
		/// <param name="e">The <see cref="WebElementEventArgs"/> instance containing the event data.</param>
		protected override void OnFindingElement(FindElementEventArgs e)
		{
			Wait.ForPageReadyStateToComplete();
			Wait.ForAngularToComplete();

			base.OnFindingElement(e);
		}

		/// <summary>
		/// To the string element.
		/// </summary>
		/// <param name="e">The <see cref="WebElementEventArgs"/> instance containing the event data.</param>
		/// <returns>Formatted issue</returns>
		private static string GetElementStringRepresentation(WebElementEventArgs e)
		{
			var stringRepresentation = string.Empty;

			try
			{
				stringRepresentation = string.Format(
					CultureInfo.CurrentCulture,
					$"{e.Element.TagName}" +
					$"{AppendAttribute(e, "id")}" +
					$"{AppendAttribute(e, "name")}" +
					$"{AppendAttribute(e, "value")}" +
					$"{AppendAttribute(e, "class")}" +
					$"{AppendAttribute(e, "type")}" +
					$"{AppendAttribute(e, "role")}" +
					$"{AppendAttribute(e, "text")}" +
					$"{AppendAttribute(e, "href")}");
			}
			catch (Exception exception)
			{
				Log.Debug(exception.Message, exception);
			}

			return stringRepresentation;
		}


		/// <summary>
		/// Appends the attribute.
		/// </summary>
		/// <param name="e">The <see cref="WebElementEventArgs"/> instance containing the event data.</param>
		/// <param name="attribute">The attribute.</param>
		/// <returns>Attribute and value</returns>
		private static string AppendAttribute(WebElementEventArgs e, string attribute)
		{
			bool isTextAttribute = attribute == "text";

			string attributeValue;

			if (isTextAttribute)
			{
				attributeValue = e.Element.Text;
			}
			else
			{
				attributeValue = e.Element.GetAttribute(attribute);
			}

			bool isAttributeNotEmpty = string.IsNullOrEmpty(attributeValue);

			string formattedAttribute;

			if (isAttributeNotEmpty)
			{
				formattedAttribute = string.Empty;
			}
			else
			{
				formattedAttribute = string.Format(CultureInfo.CurrentCulture, " {0}='{1}' ", attribute, attributeValue);
			}

			return formattedAttribute;
		}
	}
}
