using AutomationFramework.Core.Enums;
using System;
using System.Collections.Generic;

namespace AutomationFramework.Core.Utils.TestautoServer.Models.Sql
{
	public class SqlCommandBuilder
	{
		public MySqlRequestType RequestType { get; set; }

		public string Table { get; private set; }

		public string Columns { get; set; }

		public List<SqlJoin> Join { get; set; }

		public string Where { get; set; }

		public string OrderBy { get; set; }

		public string GroupBy { get; set; }

		public string Having { get; set; }

		public string Values { get; set; }

		public string Set { get; set; }

		public string Limit { get; set; }

		public Dictionary<string, object> Parameters { get; private set; }

		public SqlCommandBuilder(string table)
		{
			Table = table;
			Columns = "*";
			Join = new List<SqlJoin>();
			Where = string.Empty;
			OrderBy = string.Empty;
			GroupBy = string.Empty;
			Having = string.Empty;
			Values = string.Empty;
			Set = string.Empty;
			Limit = string.Empty;

			Parameters = new Dictionary<string, object>();
		}

		public void AddParameter(string key, object value)
		{
			Parameters.Add(key, value);
		}

		public override string ToString()
		{
			string str = "";

			switch (RequestType)
			{
				case MySqlRequestType.Select:
					str += $"SELECT {Columns} FROM {Table}";
					str += BuildRequestJoin();
					str += BuildRequestWhere();
					str += BuildRequestGroupBy();
					str += BuildRequestHaving();
					str += BuildRequestOrderBy();
					str += BuildRequestLimit();
					break;

				case MySqlRequestType.Insert:
					if (Values == String.Empty || Columns == String.Empty)
					{
						throw new Exception("Need to define 'Values' and 'Columns' for INSERT request");
					}
					str += $"INSERT INTO {Table} ({Columns}) VALUES ({Values})";
					break;

				case MySqlRequestType.Update:
					if (Set == String.Empty || Where == String.Empty)
					{
						throw new Exception("Need to define 'Set' and 'Where' for UPDATE request");
					}

					str += $"UPDATE {Table} SET {Set}";
					str += BuildRequestWhere();
					break;

				case MySqlRequestType.Delete:
					if (Where == String.Empty)
					{
						throw new Exception("Need to define 'Where' for DELETE request");
					}

					str += $"DELETE FROM {Table}";
					str += BuildRequestWhere();
					break;
			}

			return str;
		}

		private string BuildRequestJoin()
		{
			var str = "";
			foreach (var sqlJoin in Join)
			{
				str += sqlJoin.ToString();
			}

			return str;
		}

		private string BuildRequestWhere()
		{
			var str = "";
			if (Where != String.Empty)
			{
				str += $" WHERE {Where}";
			}

			return str;
		}

		private string BuildRequestGroupBy()
		{
			var str = "";
			if (GroupBy != String.Empty)
			{
				str += $" GROUP BY {GroupBy}";
			}

			return str;
		}

		private string BuildRequestHaving()
		{
			var str = "";
			if (Having != String.Empty)
			{
				str += $" HAVING {Having}";
			}

			return str;
		}

		private string BuildRequestOrderBy()
		{
			var str = "";
			if (OrderBy != String.Empty)
			{
				str += $" ORDER BY {OrderBy}";
			}

			return str;
		}

		private string BuildRequestLimit()
		{
			var str = "";
			if (Limit != String.Empty)
			{
				str += $" LIMIT {Limit}";
			}

			return str;
		}
	}
}
