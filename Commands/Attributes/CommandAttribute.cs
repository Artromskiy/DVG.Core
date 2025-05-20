using System;

namespace DVG.Core.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class CommandAttribute : Attribute { }
}
