using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using SampleTest;
using SampleTest.Generated.EnumExtensions;

namespace Benchmark
{

    [MemoryDiagnoser]
    public class BenchMarks
    {
        [Benchmark]
        public void EnumToString_Generated_JsonPropertyName()
        {
            JsonPropertyEnum.One.GetString();
            JsonPropertyEnum.Two.GetString();
            JsonPropertyEnum.Three.GetString();
            JsonPropertyEnum.Four.GetString();
            JsonPropertyEnum.Five.GetString();

        }

        [Benchmark]
        public void EnumToString_JsonPropertyName()
        {
            Enum.GetName(JsonPropertyEnum.One);
            Enum.GetName(JsonPropertyEnum.Two);
            Enum.GetName(JsonPropertyEnum.Three);
            Enum.GetName(JsonPropertyEnum.Four);
            Enum.GetName(JsonPropertyEnum.Five);
        }
        
        [Benchmark]
        public void EnumToString_Generated_EnumMemberName()
        {
            EnumMemberEnum.Monday.GetString(); // lowercase on purpose
            EnumMemberEnum.Tuesday.GetString();
            EnumMemberEnum.Wednesday.GetString();
            EnumMemberEnum.Thursday.GetString();
            EnumMemberEnum.Friday.GetString();
            EnumMemberEnum.Saturday.GetString();
            EnumMemberEnum.Sunday.GetString();
        }

        [Benchmark]
        public void EnumToString_EnumMemberName()
        {
            Enum.GetName(EnumMemberEnum.Monday);
            Enum.GetName(EnumMemberEnum.Tuesday);
            Enum.GetName(EnumMemberEnum.Wednesday);
            Enum.GetName(EnumMemberEnum.Thursday);
            Enum.GetName(EnumMemberEnum.Friday);
            Enum.GetName(EnumMemberEnum.Saturday);
            Enum.GetName(EnumMemberEnum.Sunday);
        }
    }
}
