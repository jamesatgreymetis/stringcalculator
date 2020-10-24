using System;
using FluentAssertions;
using Xunit;

namespace StringCalculator.Tests
{
    public class StringCalculatorTests
    {
        private readonly StringCalculator _sut;

        public StringCalculatorTests()
        {
            _sut = new StringCalculator();
        }

        [Theory]
        [InlineData("", 0)]
        [InlineData("1", 1)]
        [InlineData("2", 2)]
        void Single_number_input_should_return_number(string input, int expectedOutput)
        {
            // Act
            var result = _sut.Add(input);

            // Assert
            result.Should().Be(expectedOutput);
        }

        [Fact]
        void Comma_delimited_number_sequence_input_should_return_the_sum()
        {
            var input = "1,2,3";
            var expectedOutput = 6;
            
            // Act
            var result = _sut.Add(input);

            // Assert
            result.Should().Be(expectedOutput);
        }

        [Fact]
        void number_sequence_has_newline_character_should_return_the_sum()
        {
            // Arrange
            var input = "1\n2,3";
            var expectedOutput = 6;

            // Act
            var result = _sut.Add(input);

            // Assert
            result.Should().Be(expectedOutput);
        }

        [Fact]
        void invalid_number_sequence_should_throw_exception()
        {
            // Arrange
            var input = "1,\n";

            // Act
            Action act = () => _sut.Add(input);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Invalid character in sequence.");
        }

        [Theory]
        [InlineData("//;\n1;2", 3)] // single delimiter
        [InlineData("//*%\n1*2%3", 6)] // multiple delimiter
        void Custom_delimiter_with_valid_number_sequence_should_return_the_sum(string input,int expectedResult)
        {
            // Act
            var result = _sut.Add(input);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("1,2,3,", "-4")]
        [InlineData("1,2,3,", "-4,-5,-6")]
        void negative_number_in_number_sequence_should_throw_exception(string validSequence, string negativeNumbers)
        {
            // Arrange
            var input = $"{validSequence}{negativeNumbers}";

            // Act
            Action act = () => _sut.Add(input);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage($"Negatives not allowed: {negativeNumbers}");
        }

        [Theory]
        [InlineData("1001,2,13", 15)]
        [InlineData("2,1001,13", 15)]
        [InlineData("2,13,1001", 15)]
        void number_greater_than_1000_in_number_sequence_should_be_excluded_from_sum(string input, int expectedResult)
        {
            // Act
            var result = _sut.Add(input);

            // Assert
            result.Should().Be(expectedResult);
        }
    }
}
