namespace EnumToStringGenerator.Definitions;

internal struct EnumMemberDefinition
{
    public string Identifier { get; }
    public string Value { get;}

    internal EnumMemberDefinition(string identifier, string value)
    {
        Identifier = identifier;
        Value = value;
    }
}
