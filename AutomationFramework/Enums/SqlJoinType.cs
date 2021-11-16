using System.ComponentModel;

namespace AutomationFramework.Core.Enums
{
	public enum SqlJoinType
	{
		[Description("JOIN")]
		Join,
		[Description("LEFT JOIN")]
		LeftJoin,
		[Description("RIGHT JOIN")]
		RightJoin,
		[Description("FULL OUTER JOIN")]
		FullJoin,
	}
}
