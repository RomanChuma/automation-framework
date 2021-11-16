using System.Linq;
using System.Reflection;
using System.Web;

using AutomationFramework.Core.Attributes;

namespace AutomationFramework.Core.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="System.Object"/>
	/// </summary>
	public static class ObjectExtensions
	{
		/// <summary>
		/// Creates query string from <see cref="System.Object"/>
		/// </summary>
		/// <param name="sourceObject">Source object</param>
		/// <returns>Query string</returns>
		public static string ToQueryString(this object sourceObject)
		{
			var properties = sourceObject.GetType().GetProperties().Where(p => p.GetValue(sourceObject, null) != null).Select(
				propertyInfo =>
					{
						string parameterName = GetParameterName(propertyInfo);
						string query = $"{parameterName}={HttpUtility.UrlEncode(propertyInfo.GetValue(sourceObject, null).ToString())}";
						return query;
					});
			string queryString = string.Join("&", properties.ToArray());
			return queryString;
		}

		private static string GetParameterName(PropertyInfo property)
		{
			string propertyName;
			var webFormsName = (WebFormsNameAttribute)property.GetCustomAttributes(typeof(WebFormsNameAttribute), false).FirstOrDefault();
			bool webFormsNameAttributeIsDefined = webFormsName != null;

			if (webFormsNameAttributeIsDefined)
			{
				propertyName = webFormsName.Value;
			}
			else
			{
				propertyName = property.Name;
			}

			return propertyName;
		}
	}
}