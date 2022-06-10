namespace EnumToStringGenerator.Definitions;

internal struct EnumDefinition
{
    public string Identifier { get;}
    public HashSet<EnumMemberDefinition> EnumMemberDefinitions { get;}

    internal EnumDefinition(string identifier)
    {
        Identifier = identifier;
        EnumMemberDefinitions = new();
    }
}
