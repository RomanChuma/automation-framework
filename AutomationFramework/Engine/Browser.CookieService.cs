using System.Collections.Generic;

using OpenQA.Selenium;

namespace AutomationFramework.Core.Engine
{
    public partial class Browser
    {
        /// <summary>
        /// Delete all browser cookies
        /// </summary>
        public static void DeleteAllCookies()
        {
            Instance.Manage().Cookies.DeleteAllCookies();
        }

        /// <summary>
        /// Get cookie
        /// <param name="cookieName">Cookie name</param>
        /// </summary>
        /// <returns>Cookie value</returns>
        public static string GetCookieByName(string cookieName)
        {
            var cookie = Instance.Manage().Cookies.GetCookieNamed(cookieName);
            return cookie.Value;
        }

	    /// <summary>
	    /// Get all cookies
	    /// </summary>
	    /// <returns>Collection of cookies</returns>
	    public static IReadOnlyCollection<Cookie> GetCookies()
	    {
		    return Instance.Manage().Cookies.AllCookies;
	    }

		/// <summary>
		/// Add cookie
		/// </summary>
		/// <param name="cookieName"></param>
		/// <param name="cookieValue"></param>
		public static void AddCookie(string cookieName, string cookieValue)
        {
            var cookie = new Cookie(cookieName, cookieValue);
            Instance.Manage().Cookies.AddCookie(cookie);
        }

        /// <summary>
        /// Delete cookie by name
        /// </summary>
        /// <param name="cookieName">Cookie name</param>
        public static void DeleteCookie(string cookieName)
        {
            Instance.Manage().Cookies.DeleteCookieNamed(cookieName);
        }
    }
}
