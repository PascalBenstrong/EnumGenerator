namespace EnumToStringGenerator.Definitions;

internal struct EnumMethodDefinition
{
    public string MethodName { get; internal set; }
    public string EnumType { get; internal set; }
    public EnumMethodContentDefinition Content { get; internal set; }
    public string ReturnType { get; internal set; }

    private const string METHOD_MODIFIERS = "public static ";
    internal void WriteTo(StringBuilder sb, int indent)
    {
        var indenting = ClassDefinition.TAB.Repeat(indent);

        sb.Append(indenting).Append(METHOD_MODIFIERS).Append(ReturnType).Append(" ").Append(MethodName);

        if(ReturnType != EnumType)
        {
            // to string

            sb.Append("(this ").Append(EnumType).AppendLine(" @enum)");
            sb.Append(indenting).AppendLine("{");

            Content.WriteTo(sb, "@enum", indent + 1);

            sb.Append(indenting).AppendLine("}");
        }
        else
        {
            // string to enum

            sb.Append("(this string").AppendLine(" @value)");
            sb.Append(indenting).AppendLine("{");

            Content.WriteTo(sb, "@value", indent + 1, false);

            sb.Append(indenting).AppendLine("}");
        }

        sb.AppendLine();
    }
}
