namespace EnumToStringGenerator.Definitions;

internal struct NameSpaceDefinition
{
    public string NameSpace { get; internal set; }
    public IEnumerable<string> Usings { get; internal set; }
    public IEnumerable<ClassDefinition> ClassDefinitions { get; internal set; }

    internal string GetText()
    {
        StringBuilder sb = new();

        foreach (var @using in Usings)
            sb.AppendLine(@using);

        sb.AppendLine();

        sb.Append("namespace ").AppendLine(NameSpace).AppendLine("{");

        foreach(var @class in ClassDefinitions)
        {
            @class.WriteTo(sb, 1);
        }

        sb.Append("}");

        return sb.ToString();
    }
}
