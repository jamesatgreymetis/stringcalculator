using System;
using System.Linq;

namespace StringCalculator
{
    public class StringCalculator
    {
        /// <summary>
        /// The default delimiter used if a custom delimiter is not specified.
        /// </summary>
        public const char DefaultDelimiter = ',';

        private const string CustomerDelimiterIndicator = "//";

        /// <summary>
        /// Adds a delimited sequence of numbers.
        /// </summary>
        /// <param name="input">the input to sum.</param>
        /// <returns>sum of the input.</returns>
        /// <exception cref="ArgumentException">Thrown if the input contains negative numbers or an invalid sequence.</exception>
        public int Add(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return 0;
            }

            var delimiter = ResolveDelimiter(input);

            var cleanedInput = CleanInput(input, delimiter);

            ValidateInputSequence(cleanedInput);

            var intArray = ConvertToIntArray(cleanedInput, delimiter);

            ValidateArrayForNegativeNumbers(intArray);

            return Sum(intArray);
        }

        private static int Sum(int[] intArray)
        {
            return intArray.Where(i=> i <= 1000).Sum();
        }

        /// <summary>
        /// Converts the string input to an int array
        /// </summary>
        /// <param name="cleanedInput">the cleans input to convert.</param>
        /// <param name="delimiter">the delimiters in the input.</param>
        /// <returns></returns>
        private static int[] ConvertToIntArray(string cleanedInput, char[] delimiter)
        {
            return cleanedInput
                .Split(delimiter)
                .Select(int.Parse)
                .ToArray();
        }

        /// <summary>
        /// Validates sequence for negative numbers.
        /// </summary>
        /// <param name="array">The array to validate.</param>
        /// <exception cref="ArgumentException">Thrown if the array contains negative numbers.</exception>
        private void ValidateArrayForNegativeNumbers(int[] array)
        {
            if (array.Any(i => i < 0))
            {
                throw new ArgumentException($"Negatives not allowed: {string.Join(',', array.Where(i=>i < 0).Select(i=>i))}");
            }
        }

        /// <summary>
        /// Resolves if the input has a custom delimiter specified.
        /// </summary>
        /// <param name="input">the input to analyse</param>
        /// <returns>Returns rue if a custom delimiter has been specified.</returns>
        private bool ContainsCustomDelimiter(string input)
        {
            return input.StartsWith(CustomerDelimiterIndicator);
        }

        /// <summary>
        /// Gets the customer delimiter(s) or returns the default delimiter.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>An array of delimiters</returns>
        private char[] ResolveDelimiter(string input)
        {
            if (ContainsCustomDelimiter(input))
            {
                var delimiter1 = char.Parse(input.Substring(2, 1));

                if (input.IndexOf('\n') == 4)
                {
                    var delimiter2 = char.Parse(input.Substring(3, 1));
                    return new[] { delimiter1, delimiter2 };
                }

                return new[] {delimiter1};
            }

            return new[] {DefaultDelimiter};
        }

        /// <summary>
        /// Remove the custom delimiter from the input and converts newline characters to a valid delimiter.
        /// </summary>
        /// <param name="input">The input to be cleaned.</param>
        /// <param name="delimiter">The delimiter(s) in the sequence.</param>
        /// <returns>A cleaned sequence of delimited numbers</returns>
        private string CleanInput(string input, char[] delimiter)
        {
            if (ContainsCustomDelimiter(input))
            {
                input = input.Replace($"{CustomerDelimiterIndicator}{string.Concat(delimiter)}\n", "");
            }

            return input.Replace('\n', DefaultDelimiter);
        }

        /// <summary>
        /// Validated the input has a valid sequence.
        /// </summary>
        /// <param name="input">the input to validate.</param>
        /// <exception cref="ArgumentException">Thrown is the sequence is not valid.</exception>
        private void ValidateInputSequence(string input)
        {
            if (input.EndsWith(DefaultDelimiter))
            {
                throw new ArgumentException("Invalid character in sequence.");
            }
        }
    }
}
