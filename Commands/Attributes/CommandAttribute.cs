using System;

namespace DVG.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class CommandAttribute : Attribute
    {
        public readonly bool Predicted;

        public CommandAttribute(bool predicted)
        {
            Predicted = predicted;
        }
    }
}
