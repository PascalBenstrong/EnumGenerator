
using EnumToStringGenerator.Definitions;

namespace EnumToStringGenerator;

internal class EnumSyntaxContextReceiver : ISyntaxContextReceiver
{
    private List<EnumGenDefinition> _enumDeclarations = new();
    private HashSet<string> _usingNamespaces = new();
    private GenerateStringFor? _generateFor;
    public IEnumerable<EnumDeclarationSyntax> EnumDeclarations => (!_generateFor.HasValue || _generateFor == GenerateStringFor.All)
        ? _enumDeclarations.Select(x => x.DeclarationSyntax) 
        : _enumDeclarations.Where(x => x.Type == _generateFor).Select(x => x.DeclarationSyntax);

    public IEnumerable<string> UsingNamespaces => _usingNamespaces;
    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        
        if(context.Node is CompilationUnitSyntax compilationDeclaration)
        {
            var attribute = compilationDeclaration.AttributeLists.SelectMany(x => x.Attributes)
                .FirstOrDefault(x => context.SemanticModel.GetSymbolInfo(x).Symbol.ContainingType.Name == typeof(GenerateEnumStringsForAttribute).Name);
            if(attribute != null)
            {
                if (_generateFor != null)
                    throw new NotSupportedException($"Only one {typeof(GenerateEnumStringsForAttribute)} is allowed per assembly!");


                var name = attribute.ArgumentList == null ? null : context.SemanticModel.GetSymbolInfo(attribute.ArgumentList.Arguments.FirstOrDefault()?.Expression).Symbol?.Name;

                _generateFor = name == nameof(GenerateStringFor.MarkedEnums) ? GenerateStringFor.MarkedEnums : GenerateStringFor.All;
            }

        }

        else if (context.Node is EnumDeclarationSyntax enumDeclaration)
        {
            var enumDef = context.SemanticModel.GetDeclaredSymbol(context.Node)!;
            var shouldGenStrings = enumDef.GetAttributes().Any(x => x.AttributeClass.Name == typeof(GenerateStringsAttribute).Name);
            _enumDeclarations.Add(new() { DeclarationSyntax = enumDeclaration, Type = shouldGenStrings ? GenerateStringFor.MarkedEnums : null });
            _usingNamespaces.Add($"using {CreateNamespace(enumDef.ContainingNamespace)};");

        }
    }

    internal static string CreateNamespace(INamespaceSymbol symbol)
    {
        if(symbol == null)
            return null;

        StringBuilder builder = new(symbol.Name);

        while((symbol = symbol.ContainingNamespace) != null && !string.IsNullOrWhiteSpace(symbol.Name))
        {
            builder.Insert(0, symbol.Name+".");
        }
        return builder.ToString();
    }

}
