namespace EnumToStringGenerator.Definitions;

internal struct EnumMethodContentDefinition
{
    public IEnumerable<EnumMemberDefinition> Members { get; }
    
    internal EnumMethodContentDefinition(IEnumerable<EnumMemberDefinition> members)
    {
        Members = members;
    }

    private const string EXCEPTION_TEXT = "_ => throw new ArgumentOutOfRangeException(\"Invalid argument\")";
    private const string COMMENT = " // this should not happen";
    private const string DOUBLE_QOUTES = "\"";
    internal void WriteTo(StringBuilder sb, string paramName, int indent, bool asString = true)
    {
        var indenting = ClassDefinition.TAB.Repeat(indent);

        sb.Append(indenting).Append("return ").Append(paramName).AppendLine(" switch {");

        if (asString)
        {
            foreach (var member in Members)
            {
                sb.Append(indenting).Append(ClassDefinition.TAB).Append(member.Identifier).Append(" => ");
                sb.Append(DOUBLE_QOUTES).Append(member.Value).Append(DOUBLE_QOUTES).AppendLine(",");
            }
            sb.Append(indenting).Append(ClassDefinition.TAB).Append(EXCEPTION_TEXT).AppendLine(COMMENT);
        }
        else
        {
            foreach (var member in Members)
            {
                sb.Append(indenting).Append(ClassDefinition.TAB);
                sb.Append(DOUBLE_QOUTES).Append(member.Value).Append(DOUBLE_QOUTES);
                sb.Append(" => ").Append(member.Identifier).AppendLine(",");
            }

            sb.Append(indenting).Append(ClassDefinition.TAB).AppendLine(EXCEPTION_TEXT);
        }

        sb.Append(indenting).Append(ClassDefinition.TAB).AppendLine("};").AppendLine();

    }
}