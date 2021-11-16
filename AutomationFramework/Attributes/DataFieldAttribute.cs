using System;

namespace AutomationFramework.Core.Attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public sealed class DataFieldAttribute : Attribute
	{
		public DataFieldAttribute(string name)
		{
			Name = name;
		}

		public string Name { get; }
	}
}
