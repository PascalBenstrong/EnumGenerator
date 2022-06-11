using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using FluentAssertions;
using SampleTest;
using SampleTest.Extensions;
using Xunit;
using System;

namespace UnitTest.EnumGenerator
{

    public class UnitTests
    {
        [Fact]
        public void Test_JsonPropertyEnum_AsString_Should_Pass()
        {

            JsonPropertyEnum.One.AsString().Should().Be("One");
            JsonPropertyEnum.Two.AsString().Should().Be("Two");
            JsonPropertyEnum.Three.AsString().Should().Be("three");
            JsonPropertyEnum.Four.AsString().Should().Be("Four");
            JsonPropertyEnum.Five.AsString().Should().Be("Five");
        }
        
        [Fact]
        public void Test_JsonPropertyEnum_AsJsonPropertyEnum_Should_Pass()
        {

            "One".AsJsonPropertyEnum().Should().Be(JsonPropertyEnum.One);
            "Two".AsJsonPropertyEnum().Should().Be(JsonPropertyEnum.Two);
            "three".AsJsonPropertyEnum().Should().Be(JsonPropertyEnum.Three);
            "Four".AsJsonPropertyEnum().Should().Be(JsonPropertyEnum.Four);
            "Five".AsJsonPropertyEnum().Should().Be(JsonPropertyEnum.Five);
            Action act = () => "some invalid value".AsJsonPropertyEnum();

            act.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Test_EnumMemberEnum__AsString_Should_Pass()
        {
            EnumMemberEnum.Monday.AsString().Should().Be("monday"); // lowercase on purpose
            EnumMemberEnum.Tuesday.AsString().Should().Be("Tuesday");
            EnumMemberEnum.Wednesday.AsString().Should().Be("Wednesday");
            EnumMemberEnum.Thursday.AsString().Should().Be("Thursday");
            EnumMemberEnum.Friday.AsString().Should().Be("Friday");
            EnumMemberEnum.Saturday.AsString().Should().Be("Saturday");
            EnumMemberEnum.Sunday.AsString().Should().Be("Sunday");
        }
        [Fact]
        public void Test_EnumMemberEnum_Should_Pass()
        {
            "monday".AsEnumMemberEnum().Should().Be(EnumMemberEnum.Monday); // lowercase on purpose
            "Tuesday".AsEnumMemberEnum().Should().Be(EnumMemberEnum.Tuesday);
            "Wednesday".AsEnumMemberEnum().Should().Be(EnumMemberEnum.Wednesday);
            "Thursday".AsEnumMemberEnum().Should().Be(EnumMemberEnum.Thursday);
            "Friday".AsEnumMemberEnum().Should().Be(EnumMemberEnum.Friday);
            "Saturday".AsEnumMemberEnum().Should().Be(EnumMemberEnum.Saturday);
            "Sunday".AsEnumMemberEnum().Should().Be(EnumMemberEnum.Sunday);

            Action act = () => "some invalid value".AsEnumMemberEnum();

            act.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }
    }
}