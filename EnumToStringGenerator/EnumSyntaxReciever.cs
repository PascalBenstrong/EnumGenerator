namespace EnumToStringGenerator;

internal class EnumSyntaxReciever : ISyntaxReceiver
{
    public IList<EnumDeclarationSyntax> EnumDeclarations { get; } = new List<EnumDeclarationSyntax>();
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if(syntaxNode is EnumDeclarationSyntax enumDeclaration)
        {
            var attributes = enumDeclaration.AttributeLists.SelectMany(x => x.Attributes).Any(x => x.Name.ToString() == typeof(GenerateStringsAttribute).Name);
            EnumDeclarations.Add(enumDeclaration);
        }
    }
}
