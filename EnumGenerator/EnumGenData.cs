using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EnumGenerator
{
    internal struct EnumGenData
    {
        public GenerateStringFor? Type { get; set; }
        public EnumDeclarationSyntax DeclarationSyntax { get; set; }
    }
}
