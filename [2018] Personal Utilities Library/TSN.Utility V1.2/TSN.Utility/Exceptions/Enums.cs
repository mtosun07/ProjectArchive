using System.ComponentModel;

namespace TSN.Utility.Exceptions
{
    public enum ArgumentTypes : byte
    {
        [Description("")] NONE = 0,
        [Description("string")] String = 1,
        [Description("collection")] Collection = 2,
        [Description("dictionary")] Dictionary = 3,
        [Description("array")] Array = 4
    }
    public enum Quantity : byte
    {
        Single = 0,
        Some = 1,
        All = 2,
        Plural = 3
    }
}