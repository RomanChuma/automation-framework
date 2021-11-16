using System;

namespace AutomationFramework.Core.Attributes
{
	/// <summary>
	/// Attribute that stores the value of name represented within the web forms request
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class WebFormsNameAttribute : Attribute
	{
		public WebFormsNameAttribute(string value)
		{
			Value = value;
		}

		public string Value { get; }
	}
}