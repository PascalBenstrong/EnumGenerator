namespace EnumToStringGenerator;

internal static class Utils
{
    private static Dictionary<EnumDeclarationSyntax, EnumDefinition> cache = new();

    internal static EnumDefinition GetEnumDefinition(EnumDeclarationSyntax enumDeclaration)
    {
        if(!cache.TryGetValue(enumDeclaration, out EnumDefinition definition))
        {
            definition = new(enumDeclaration.Identifier.ValueText);
            foreach(var declaration in enumDeclaration.Members)
            {
                EnumMemberDefinition memberDefinition = new(definition.Identifier + "." + declaration.Identifier.ValueText,
                    GetMemberValue(declaration));
                definition.EnumMemberDefinitions.Add(memberDefinition);
            }

            cache.Add(enumDeclaration, definition);
        }

        return definition;
    }

    internal static string GetMemberValue(EnumMemberDeclarationSyntax enumMember)
    {
        var attribute = enumMember.AttributeLists.SelectMany(x => x.Attributes)
            .FirstOrDefault(x => x.Name.ToString() == "JsonPropertyName");

        attribute = attribute is null ? enumMember.AttributeLists.SelectMany(x => x.Attributes)
            .FirstOrDefault(x => x.Name.ToString() == "EnumMember") : null;

        if (attribute is not null)
        {
            return attribute.ArgumentList.Arguments.Where(x => x.Expression.Kind() == SyntaxKind.StringLiteralExpression)
                .FirstOrDefault().Expression.ChildTokens()
                .FirstOrDefault(x => x.IsKind(SyntaxKind.StringLiteralToken)).ValueText;

        }

        return enumMember.Identifier.ValueText;
    }

    internal static string Repeat(this char c, int count)
    {
        return new string(c, count);
    }
}
