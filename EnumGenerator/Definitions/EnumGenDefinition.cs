namespace EnumToStringGenerator.Definitions;

internal struct EnumGenDefinition
{
    public GenerateStringFor? Type { get; internal set; }
    public EnumDeclarationSyntax DeclarationSyntax { get; internal set; }
}
