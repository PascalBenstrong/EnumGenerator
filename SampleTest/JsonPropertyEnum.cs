
//[assembly: GenerateEnumStringsFor(GenerateStringFor.MarkedEnums)]
namespace SampleTest
{
    //[GenerateStrings]
    public enum JsonPropertyEnum
    {
        [JsonPropertyName("One")]
        One = 1,
        [JsonPropertyName("Two")]
        Two = 2,
        [JsonPropertyName("Three")]
        Three = 3,
        [JsonPropertyName("Four")]
        Four = 4,
        [JsonPropertyName("Five")]
        Five = 5,
    }
}