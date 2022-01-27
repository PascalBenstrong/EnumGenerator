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
        private static string GetFromEnumMembers(EnumDeclarationSyntax enumSyntax)
        {
            StringBuilder sb = new();
            string identifier = enumSyntax.Identifier.ValueText;

            static string GetMemberValue(EnumMemberDeclarationSyntax enumMember)
            {
                var attributes = enumMember.AttributeLists.SelectMany(x => x.Attributes)
                    .Where(x => x.Name.ToString() == "JsonPropertyName");

                attributes = !attributes.Any() ? enumMember.AttributeLists.SelectMany(x => x.Attributes)
                    .Where(x => x.Name.ToString() == "EnumMember") : Enumerable.Empty<AttributeSyntax>();

                if (attributes.Any())
                {
                    StringBuilder sb = new ();

                    int count = 0;
                    foreach (var attribute in attributes)
                    {
                        count++;
                        sb.Append(attribute.ArgumentList.Arguments
                        .Where(x => x.Expression.Kind() == SyntaxKind.StringLiteralExpression)
                        .FirstOrDefault().Expression.ChildTokens()
                        .FirstOrDefault(x => x.IsKind(SyntaxKind.StringLiteralToken)).ValueText).Append(" |");
                    }

                    if (count > 0)
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
            var _namespace = context.Compilation.AssemblyName;
            StringBuilder sourceBuilder = new();
            sourceBuilder.Append($@"
using System;
{string.Join(Environment.NewLine, syntaxReciever.UsingNamespaces)}

namespace {_namespace}.Generated
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
}
