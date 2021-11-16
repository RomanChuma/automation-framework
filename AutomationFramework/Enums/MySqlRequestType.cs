using System.ComponentModel;

namespace AutomationFramework.Core.Enums
{
    public enum MySqlRequestType
    {
        [Description("SELECT")]
        Select,
        [Description("INSERT")]
        Insert,
        [Description("UPDATE")]
        Update,
        [Description("DELETE")]
        Delete
    }
}
