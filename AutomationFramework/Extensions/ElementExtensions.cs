using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using AutomationFramework.Core.Attributes;
using AutomationFramework.Core.Controls.Interfaces;

using OpenQA.Selenium;

namespace AutomationFramework.Core.Extensions
{
	public static class ElementExtensions
	{
		/// <summary>
		/// Convert generic IWebElement into custom web element (Checkbox, Table, etc.)
		/// </summary>
		/// <typeparam name="TElement">Specified web element class</typeparam>
		/// <param name="webElement">Generic IWebElement</param>
		/// <returns>
		/// Custom web element (Checkbox, Table, etc.)
		/// </returns>
		/// <exception cref="System.ArgumentNullException">When constructor is null</exception>
		public static TElement As<TElement>(this IWebElement webElement)
			where TElement : class, IHtmlElement
		{
			Type elementType = typeof(TElement);
			var constructor = elementType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null , new[] { typeof(IWebElement) }, null);

			if (constructor != null)
			{
				return constructor.Invoke(new object[] { webElement }) as TElement;
			}

			var errorMessage = string.Format(CultureInfo.CurrentCulture,
				$"Constructor for type '{typeof(TElement)}' is null.");
			throw new ArgumentNullException(errorMessage);
		}

		/// <summary>
		/// Get UI element name attribute value
		/// </summary>
		/// <typeparam name="T">
		/// UI Element
		/// </typeparam>
		/// <param name="expression">
		/// Element with name attribute
		/// </param>
		/// <returns>
		/// Attribute value or string.Empty
		/// </returns>
		public static string GetNameAttributeValue<T>(Expression<Func<T>> expression)
		{
			var body = (MemberExpression)expression.Body;
			var attribute = (NameAttribute)body.Member.GetCustomAttributes(typeof(NameAttribute), false).FirstOrDefault();

			if (attribute == null)
			{
				return string.Empty;
			}

			return attribute.Value;
		}

		public static string GetPageName<T>(Expression<Func<T>> expression)
			where T : IHtmlElement
		{
			var member = (MemberExpression)expression.Body;
			var pageType = member.Expression.Type;

			var pageNameAttribute = pageType.GetCustomAttribute<NameAttribute>(false);

			// Default page name
			string pageName = $"{member.Member.ReflectedType.Name}";

			if (pageNameAttribute != null)
			{
				pageName = pageNameAttribute.Value;
			}

			return pageName;
		}

		public static string GetElementName<T>(Expression<Func<T>> expression)
			where T : IHtmlElement
		{
			string elementName = GetNameAttributeValue(expression);

			if (elementName == string.Empty)
			{
				var body = (MemberExpression)expression.Body;

				// Default element name
				elementName = body.Member.Name;
			}

			return elementName;
		}
	}
}
