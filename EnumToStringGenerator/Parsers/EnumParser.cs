namespace EnumToStringGenerator.Parsers;

internal sealed class EnumParser
{
    public static IEnumerable<EnumMethodDefinition> Parse(EnumDeclarationSyntax enumDeclaration)
    {
        var enumDef = Utils.GetEnumDefinition(enumDeclaration);

        EnumMethodDefinition def = new() 
        {
            Content = new(enumDef.EnumMemberDefinitions),
            EnumType = enumDef.Identifier, 
            MethodName = "AsString", 
            ReturnType = "string"
        };

        yield return def;

        // from string

        def = new()
        {
            Content = def.Content,
            EnumType = def.EnumType,
            MethodName = $"As{def.EnumType}",
            ReturnType = def.EnumType
        };

        yield return def;
    }
}
