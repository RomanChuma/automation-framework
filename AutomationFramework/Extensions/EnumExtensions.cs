using System;
using System.ComponentModel;
using System.Reflection;

using AutomationFramework.Core.Enums;

namespace AutomationFramework.Core.Extensions
{
	/// <summary>
	/// Extension methods for Enums
	/// </summary>
	public static class EnumExtensions
	{
		/// <summary>
		/// Get boolean value of <see cref="ElementState"/>
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool BooleanValue(this ElementState value)
		{
			switch (value)
			{
				case ElementState.Checked:
					return true;
				case ElementState.Unchecked:
					return false;
				case ElementState.Disabled:
					return false;
				case ElementState.Enabled:
					return true;
				default:
					throw new ArgumentOutOfRangeException(nameof(value));
			}
		}

		/// <summary>
		/// Get boolean value of <see cref="SelectionState"/>
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool BooleanValue(this SelectionState value)
		{
			switch (value)
			{
				case SelectionState.Selected:
					return true;
				case SelectionState.Unselected:
					return false;
				default:
					throw new ArgumentOutOfRangeException(nameof(value));
			}
		}

		/// <summary>
		/// Get description attribute value
		/// </summary>
		/// <param name="enumValue">Enum value</param>
		/// <returns>String value</returns>
		public static string GetDescription(this Enum enumValue)
		{
			FieldInfo fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
			var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

			if (attributes.Length > 0)
			{
				return attributes[0].Description;
			}

			return enumValue.ToString();
		}

		/// <summary>
		/// Get random value from enum of type T
		/// </summary>
		/// <typeparam name="T">Type of enum</typeparam>
		/// <param name="enumeration">Enumeration on which the method would be called</param>
		/// <returns>Random value from given enum</returns>
		public static T GetRandomValue<T>(this Enum enumeration)
			where T : Enum
		{
			Array enumValues = Enum.GetValues(typeof(T));
			var random = new Random();
			int indexOfValueToGet = random.Next(enumValues.Length);
			var randomValue = (T)enumValues.GetValue(indexOfValueToGet);
			return randomValue;
		}

		/// <summary>
		/// Get value from description attribute
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="description"></param>
		/// <returns></returns>
		public static T GetValueFromDescription<T>(string description)
			where T : Enum
		{
			Type type = typeof(T);
			foreach (FieldInfo field in type.GetFields())
			{
				if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
				{
					if (attribute.Description == description)
					{
						return (T)field.GetValue(null);
					}
				}
				else
				{
					if (field.Name == description)
					{
						return (T)field.GetValue(null);
					}
				}
			}

			throw new ArgumentException("Not found.", "description");
		}
	}
}