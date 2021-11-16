using System;
using System.Reflection;

namespace AutomationFramework.Core.Utils
{
	public static class AttributeHelper
	{
		public static T GetCustomAttributeValue<T>(MethodBase method) where T: Attribute
		{
			T attribute = (T)method.GetCustomAttributes(typeof(T), true)[0];
			return attribute;
		}
	}
}
