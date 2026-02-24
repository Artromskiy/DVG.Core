using System;

namespace DVG
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public class InlineArrayAttribute : Attribute
    {
        public readonly int Length;

        public InlineArrayAttribute(int length)
        {
            Length = length;
        }
    }
}
