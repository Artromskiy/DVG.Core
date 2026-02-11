using System;

namespace DVG.Components.Attributes
{
    [AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class ComponentAttribute : Attribute
    {
        public readonly bool History;

        public ComponentAttribute(bool hasHistory)
        {
            History = hasHistory;
        }
    }
}
