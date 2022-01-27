using System;

namespace EnumGenerator
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public class GenerateEnumStringsForAttribute : Attribute
    {
        private readonly GenerateStringFor _generateStringFor;

        public GenerateEnumStringsForAttribute(GenerateStringFor generateStringFor = GenerateStringFor.All)
        {
            _generateStringFor = generateStringFor;
        }
    }
}
