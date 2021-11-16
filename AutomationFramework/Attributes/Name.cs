using System;

namespace AutomationFramework.Core.Attributes
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Field)]
	public class NameAttribute : Attribute
	{
		public NameAttribute(string value)
		{
			Value = value;
		}

		public string Value { get; private set; }
	}
}
