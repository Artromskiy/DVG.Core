using System;

namespace DVG.Core.History.Attributes
{
    [AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class HistoryAttribute : Attribute { }
}
