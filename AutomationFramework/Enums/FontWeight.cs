using System.ComponentModel;

namespace AutomationFramework.Core.Enums
{
	/// <summary>
	/// Font-weight with name and value
	/// Use for elements HTML style verification
	/// See reference: http://www.css3-tutorial.net/text-font/font-weight/
	/// </summary>
	public enum FontWeight
    {
        [Description("100")]
        Thin,
        [Description("200")]
        ExtraLight,
        [Description("300")]
        Light,
        [Description("400")]
        Normal,
        [Description("500")]
        Medium,
        [Description("600")]
        SemiBold,
        [Description("700")]
        Bold,
        [Description("800")]
        ExtraBold,
        [Description("900")]
        Black,
        None
    }
}
