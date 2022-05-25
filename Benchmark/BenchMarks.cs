using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using SampleTest;
using SampleTest.Extensions;

namespace Benchmark
{

    [MemoryDiagnoser]
    public class BenchMarks
    {
        [Benchmark]
        public void EnumToString_Generated_JsonPropertyName()
        {
            JsonPropertyEnum.One.AsString();
            JsonPropertyEnum.Two.AsString();
            JsonPropertyEnum.Three.AsString();
            JsonPropertyEnum.Four.AsString();
            JsonPropertyEnum.Five.AsString();

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
            EnumMemberEnum.Monday.AsString();
            EnumMemberEnum.Tuesday.AsString();
            EnumMemberEnum.Wednesday.AsString();
            EnumMemberEnum.Thursday.AsString();
            EnumMemberEnum.Friday.AsString();
            EnumMemberEnum.Saturday.AsString();
            EnumMemberEnum.Sunday.AsString();
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
