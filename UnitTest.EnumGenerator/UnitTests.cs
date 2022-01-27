using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using FluentAssertions;
using SampleTest.Generated;
using SampleTest;
using Xunit;
using System;

namespace UnitTest.EnumGenerator
{

    public class UnitTests
    {
        [Fact]
        public void Test_JsonPropertyEnum_Should_Pass()
        {

            JsonPropertyEnum.One.GetString().Should().Be("One");
            JsonPropertyEnum.Two.GetString().Should().Be("Two");
            JsonPropertyEnum.Three.GetString().Should().Be("Three");
            JsonPropertyEnum.Four.GetString().Should().Be("Four");
            JsonPropertyEnum.Five.GetString().Should().Be("Five");
        }

        [Fact]
        public void Test_EnumMemberEnum_Should_Pass()
        {
            EnumMemberEnum.Monday.GetString().Should().Be("monday"); // lowercase on purpose
            EnumMemberEnum.Tuesday.GetString().Should().Be("Tuesday");
            EnumMemberEnum.Wednesday.GetString().Should().Be("Wednesday");
            EnumMemberEnum.Thursday.GetString().Should().Be("Thursday");
            EnumMemberEnum.Friday.GetString().Should().Be("Friday");
            EnumMemberEnum.Saturday.GetString().Should().Be("Saturday");
            EnumMemberEnum.Sunday.GetString().Should().Be("Sunday");
        }
    }
}