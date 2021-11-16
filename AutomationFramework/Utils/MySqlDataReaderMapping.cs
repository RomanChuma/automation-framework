using System;
using System.Data;
using System.Globalization;
using System.Reflection;
using AutomationFramework.Core.Attributes;

namespace AutomationFramework.Core.Utils
{
	/// <summary>
	/// Class that is mapping a MySqlDataReader result to an object (containing attributes [DataField("...")])
	/// 
	/// Base on : https://www.codeproject.com/Articles/830129/Generically-Populate-a-Custom-NET-Class-from-a-Sql
	/// </summary>
	public static class MySqlDataReaderMapping
	{
		public static TEntity ReflectType<TEntity>(IDataRecord dr) where TEntity : class, new()
		{
			TEntity instanceToPopulate = new TEntity();

			PropertyInfo[] propertyInfos = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);

			//for each public property on the original
			foreach (PropertyInfo pi in propertyInfos)
			{
				DataFieldAttribute[] dataFieldAttributeArray = pi.GetCustomAttributes(typeof(DataFieldAttribute), false) as DataFieldAttribute[];

				//this attribute is marked with AllowMultiple=false
				if (dataFieldAttributeArray != null && dataFieldAttributeArray.Length == 1)
				{
					DataFieldAttribute dfa = dataFieldAttributeArray[0];

					//this will blow up if the datareader does not contain the item keyed dfa.Name
					object dbValue = dr[dfa.Name];

					if (dbValue != null)
					{
						pi.SetValue(instanceToPopulate, Convert.ChangeType(dbValue, pi.PropertyType, CultureInfo.InvariantCulture), null);
					}
				}
			}

			return instanceToPopulate;
		}
	}
}
