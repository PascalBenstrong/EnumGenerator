namespace EnumToStringGenerator.Definitions;

internal struct ClassDefinition
{
    public string Name { get; internal set; }
    public IEnumerable<EnumMethodDefinition> Methods { get; internal set; }

    private const string CLASS_SIGNATURE = "public static partial class ";
    internal const char TAB = '\t';

    internal void WriteTo(StringBuilder sb, int indent)
    {
        var indenting = TAB.Repeat(indent);
        sb.Append(indenting).Append(CLASS_SIGNATURE).AppendLine(Name);
        sb.Append(indenting).AppendLine("{");

        foreach(var method in Methods)
        {
            method.WriteTo(sb, indent+1);
        }

        sb.Append(indenting).AppendLine("}").AppendLine();
    }
}
