using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace EnumGenerator
{
    [Generator]
    public class SourceGenerator : ISourceGenerator
    {
        private static NamespaceDeclarationSyntax GetNameSpace(SyntaxNode node)
        {
            static NamespaceDeclarationSyntax GetNodeRecursive(SyntaxNode child, SyntaxNode parent)
            {
                if (child is null)
                    return null;

                if (child is NamespaceDeclarationSyntax _namespace)
                {
                    return _namespace;
                }
                return GetNodeRecursive(parent, parent?.Parent);
            }

            return GetNodeRecursive(node, node?.Parent);

        }

        private static string GetFromEnumMembers(EnumDeclarationSyntax enumSyntax)
        {
            StringBuilder sb = new();
            string identifier = enumSyntax.Identifier.ValueText;

            static string GetMemberValue(EnumMemberDeclarationSyntax enumMember)
            {
                var attributes = enumMember.AttributeLists.SelectMany(x => x.Attributes)
                    .Where(x => x.Name.ToString() == "JsonPropertyName");

                attributes ??= enumMember.AttributeLists.SelectMany(x => x.Attributes)
                    .Where(x => x.Name.ToString() == "EnumMember");

                if (attributes is not null)
                {
                    StringBuilder sb = new ();

                    foreach (var attribute in attributes)
                    {
                        sb.Append(attribute.ArgumentList.Arguments
                        .Where(x => x.Expression.Kind() == SyntaxKind.StringLiteralExpression)
                        .FirstOrDefault().Expression.ChildTokens()
                        .FirstOrDefault(x => x.IsKind(SyntaxKind.StringLiteralToken)).ValueText).Append(" |");
                    }

                    if (sb.Length > 0)
                        return sb.Remove(sb.Length - 2, 2).ToString();
                }

                return enumMember.Identifier.ValueText;
            }

            bool first = true;
            foreach (var member in enumSyntax.Members)
            {
                if (first)
                    sb.AppendLine($"\t\t\"{GetMemberValue(member)}\" => {identifier}.{member.Identifier.ValueText},");
                else
                    sb.AppendLine($"\t\t\t\t\t\"{GetMemberValue(member)}\" => {identifier}.{member.Identifier.ValueText},");
                first = false;
            }
            sb.AppendLine($"\t\t\t\t\t _ => throw new ArgumentOutOfRangeException(\"Invalid argument\")");

            return sb.ToString();
        }

        private static string GetMembers(EnumDeclarationSyntax enumSyntax)
        {
            StringBuilder sb = new();
            string identifier = enumSyntax.Identifier.ValueText;

            static string GetMemberValue(EnumMemberDeclarationSyntax enumMember)
            {
                var attribute = enumMember.AttributeLists.SelectMany(x => x.Attributes)
                    .FirstOrDefault(x => x.Name.ToString() == "JsonPropertyName");

                attribute ??= enumMember.AttributeLists.SelectMany(x => x.Attributes)
                    .FirstOrDefault(x => x.Name.ToString() == "EnumMember");

                if (attribute is not null)
                {
                    return attribute.ArgumentList.Arguments
                        .Where(x => x.Expression.Kind() == SyntaxKind.StringLiteralExpression)
                        .FirstOrDefault().Expression.ChildTokens()
                        .FirstOrDefault(x => x.IsKind(SyntaxKind.StringLiteralToken)).ValueText;
                }

                return enumMember.Identifier.ValueText;
            }
            bool first = true;
            foreach(var member in enumSyntax.Members)
            {
                if(first)
                sb.AppendLine($"\t\t{identifier}.{member.Identifier.ValueText} => \"{GetMemberValue(member)}\",");
                else
                sb.AppendLine($"\t\t\t\t\t{identifier}.{member.Identifier.ValueText} => \"{GetMemberValue(member)}\",");
                first = false;
            }
            sb.AppendLine($"\t\t\t\t\t _ => throw new ArgumentOutOfRangeException(\"Invalid argument\")");

            return sb.ToString();
        }

        private string CreateMethods(IEnumerable<EnumDeclarationSyntax> enumDeclarations)
        {
            StringBuilder sb = new();

            var first = true;

            foreach(var enumDeclaration in enumDeclarations)
            {
                if (first)
                {
sb.AppendLine(
$@"public static string GetString(this {enumDeclaration.Identifier.ValueText} @enum)
        {{
            return @enum switch {{

            {GetMembers(enumDeclaration)}
            }};
        }}"
).AppendLine()
.AppendLine(
    $@"{"\t\t"}public static {enumDeclaration.Identifier.ValueText} FromString{enumDeclaration.Identifier.ValueText}(this string @string)
        {{
            return @string switch {{
            {GetFromEnumMembers(enumDeclaration)}
            }};

        }}"
).AppendLine();
                }
                else
                {
sb.AppendLine(
$@"{"\t\t"}public static string GetString(this {enumDeclaration.Identifier.ValueText} @enum)
        {{
            return @enum switch {{

            {GetMembers(enumDeclaration)}
            }};

        }}"
).AppendLine()
.AppendLine(
    $@"{"\t\t"}public static {enumDeclaration.Identifier.ValueText} FromString{enumDeclaration.Identifier.ValueText}(this string @string)
        {{
            return @string switch {{
            {GetFromEnumMembers(enumDeclaration)}
            }};

        }}"
).AppendLine();
                }

                first = false;
            }

            return sb.ToString();
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var syntaxReciever = (EnumSyntaxContextReceiver)context.SyntaxContextReceiver;
            IEnumerable<EnumDeclarationSyntax> enumDeclarations = syntaxReciever.EnumDeclarations;

            if (!enumDeclarations.Any())
                return;
            var _namespace = GetNameSpace(enumDeclarations.First()).Name;
            StringBuilder sourceBuilder = new();
            sourceBuilder.Append($@"
using System;
{string.Join(Environment.NewLine, syntaxReciever.UsingNamespaces)}

namespace {_namespace}.Generated.EnumExtensions
{{
    public static partial class EnumExtensions
    {{
        {CreateMethods(enumDeclarations)}
    }}
}}
");

            context.AddSource($"EnumGenerator_{enumDeclarations.First().Identifier.ValueText}", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));

        }

        public void Initialize(GeneratorInitializationContext context)
        {
            //context.RegisterForSyntaxNotifications(() => new EnumSyntaxReciever());
            context.RegisterForSyntaxNotifications(() => new EnumSyntaxContextReceiver());

#if DEBUG
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
#endif
        }
    }

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

    internal class EnumSyntaxContextReceiver : ISyntaxContextReceiver
    {
        private List<EnumGenData> _enumDeclarations = new();
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


    internal struct EnumGenData
    {
        public GenerateStringFor? Type { get; set; }
        public EnumDeclarationSyntax DeclarationSyntax { get; set; }
    }
}
