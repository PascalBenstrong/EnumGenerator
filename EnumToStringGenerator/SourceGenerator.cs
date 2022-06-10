namespace EnumToStringGenerator;

[Generator]
public class SourceGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        var syntaxReciever = (EnumSyntaxContextReceiver)context.SyntaxContextReceiver;
        IEnumerable<EnumDeclarationSyntax> enumDeclarations = syntaxReciever.EnumDeclarations;

        if (!enumDeclarations.Any())
            return;

        var defs = enumDeclarations.SelectMany(x => EnumParser.Parse(x));

        ClassDefinition classDefinition = new() { Name = "EnumExtensions", Methods = defs };

        var _namespace = context.Compilation.AssemblyName;
        NameSpaceDefinition nameSpaceDefinition = new()
        {
            ClassDefinitions = new[] { classDefinition },
            NameSpace = $"{_namespace}.Extensions",
            Usings = syntaxReciever.UsingNamespaces
        };


        context.AddSource($"EnumGenerator_Extensions", SourceText.From(nameSpaceDefinition.GetText(), Encoding.UTF8));

    }

    public void Initialize(GeneratorInitializationContext context)
    {
        //context.RegisterForSyntaxNotifications(() => new EnumSyntaxReciever());
        context.RegisterForSyntaxNotifications(() => new EnumSyntaxContextReceiver());

#if DEBUG
        if (!Debugger.IsAttached)
        {
            //Debugger.Launch();
        }
#endif
    }
}
