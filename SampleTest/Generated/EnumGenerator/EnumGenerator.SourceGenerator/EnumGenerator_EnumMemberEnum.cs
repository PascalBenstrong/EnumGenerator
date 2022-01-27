
using System;
using SampleTest;

namespace SampleTest.Generated.EnumExtensions
{
    public static partial class EnumExtensions
    {
        public static string GetString(this EnumMemberEnum @enum)
        {
            return @enum switch {

            		EnumMemberEnum.Monday => "monday",
					EnumMemberEnum.Tuesday => "Tuesday",
					EnumMemberEnum.Wednesday => "Wednesday",
					EnumMemberEnum.Thursday => "Thursday",
					EnumMemberEnum.Friday => "Friday",
					EnumMemberEnum.Saturday => "Saturday",
					EnumMemberEnum.Sunday => "Sunday",
					 _ => throw new ArgumentOutOfRangeException("Invalid argument")

            };
        }

		public static EnumMemberEnum FromStringEnumMemberEnum(this string @string)
        {
            return @string switch {
            		"Monday" => EnumMemberEnum.Monday,
					"Tuesday" => EnumMemberEnum.Tuesday,
					"Wednesday" => EnumMemberEnum.Wednesday,
					"Thursday" => EnumMemberEnum.Thursday,
					"Friday" => EnumMemberEnum.Friday,
					"Saturday" => EnumMemberEnum.Saturday,
					"Sunday" => EnumMemberEnum.Sunday,
					 _ => throw new ArgumentOutOfRangeException("Invalid argument")

            };

        }

		public static string GetString(this JsonPropertyEnum @enum)
        {
            return @enum switch {

            		JsonPropertyEnum.One => "One",
					JsonPropertyEnum.Two => "Two",
					JsonPropertyEnum.Three => "Three",
					JsonPropertyEnum.Four => "Four",
					JsonPropertyEnum.Five => "Five",
					 _ => throw new ArgumentOutOfRangeException("Invalid argument")

            };

        }

		public static JsonPropertyEnum FromStringJsonPropertyEnum(this string @string)
        {
            return @string switch {
            		"One" => JsonPropertyEnum.One,
					"Two" => JsonPropertyEnum.Two,
					"Three" => JsonPropertyEnum.Three,
					"Four" => JsonPropertyEnum.Four,
					"Five" => JsonPropertyEnum.Five,
					 _ => throw new ArgumentOutOfRangeException("Invalid argument")

            };

        }


    }
}
