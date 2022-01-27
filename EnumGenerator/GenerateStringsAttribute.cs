using System;
using System.Collections.Generic;
using System.Text;

namespace EnumGenerator
{
    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = false, Inherited = true)]
    public class GenerateStringsAttribute : Attribute
    {
    }
}
