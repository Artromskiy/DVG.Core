using System;

namespace DVG.Ids.Attributes
{
    [AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class IdAttribute : Attribute
    {
        public readonly bool UseInt;

        public IdAttribute(bool useInt)
        {
            UseInt = useInt;
        }
    }
}