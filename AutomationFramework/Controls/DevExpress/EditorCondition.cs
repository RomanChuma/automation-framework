using System.ComponentModel;

namespace AutomationFramework.Core.Controls.DevExpress
{
	/// <summary>
	/// Conditions available within <see cref="AspXRichEditComponent"/>
	/// </summary>
	public enum EditorCondition
	{
		/// <summary>
		/// [IF ... THEN]...[END]
		/// </summary>
		[Description("[IF ... THEN]...[END]")]
		IfThenEnd = 0,

		/// <summary>
		/// [IF ... THEN]...[ELSE]...[END]
		/// </summary>
		[Description("[IF ... THEN]...[ELSE]...[END]")]
		IfThenElseEnd = 1,

		/// <summary>
		/// [IF ... THEN]...[ELSE IF ... THEN]...[END]
		/// </summary>
		[Description("[IF ... THEN]...[ELSE IF ... THEN]...[END]")]
		IfThenElseIfThenEnd = 2,

		/// <summary>
		/// [IF ... THEN]...[ELSE IF ... THEN]...[ELSE]...[END]
		/// </summary>
		[Description("[IF ... THEN]...[ELSE IF ... THEN]...[ELSE]...[END]")]
		IfThenElseIfThenElseEnd = 3,

		/// <summary>
		/// [ELSE IF ... THEN]
		/// </summary>
		[Description("[ELSE IF ... THEN]")]
		ElseIfThen = 4
	}
}