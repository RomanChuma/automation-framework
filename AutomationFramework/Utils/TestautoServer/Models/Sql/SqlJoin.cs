using AutomationFramework.Core.Enums;
using AutomationFramework.Core.Extensions;

namespace AutomationFramework.Core.Utils.TestautoServer.Models.Sql
{
	public class SqlJoin
	{
		public string Table { get; set; }

		public string On { get; set; }

		public SqlJoinType JoinType { get; set; }


		public SqlJoin(SqlJoinType joinType, string table, string on)
		{
			JoinType = joinType;
			Table = table;
			On = on;
		}

		public override string ToString()
		{
			return $" {JoinType.GetDescription()} {Table} ON {On}";
		}
	}
}
