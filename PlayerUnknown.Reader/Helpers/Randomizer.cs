namespace PlayerUnknown.Reader.Helpers
{
    using System;
    using System.Text;

    /// <summary>
    /// Static helper class providing tools for generating random numbers or strings.
    /// </summary>
    public static class Randomizer
    {
        /// <summary>
        /// Provides random engine.
        /// </summary>
        private static readonly Random Random = new Random();

        /// <summary>
        /// Allowed characters in random strings.
        /// </summary>
        private static readonly char[] AllowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

        /// <summary>
        /// Returns a random number within a specified range.
        /// </summary>
        /// <param name="MinValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="MaxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns>A 32-bit signed integer greater than or equal to minValue and less than maxValue.</returns>
        public static int GenerateNumber(int MinValue, int MaxValue)
        {
            return Randomizer.Random.Next(MinValue, MaxValue);
        }

        /// <summary>
        /// Returns a nonnegative random number less than the specified maximum.
        /// </summary>
        /// <param name="MaxValue">The exclusive upper bound of the random number to be generated. maxValue must be greater than or equal to zero.</param>
        /// <returns>A 32-bit signed integer greater than or equal to zero, and less than maxValue.</returns>
        public static int GenerateNumber(int MaxValue)
        {
            return Randomizer.Random.Next(MaxValue);
        }

        /// <summary>
        /// Returns a nonnegative random number.
        /// </summary>
        /// <returns>A 32-bit signed integer greater than or equal to zero and less than <see cref="int.MaxValue"/>.</returns>
        public static int GenerateNumber()
        {
            return Randomizer.Random.Next();
        }

        /// <summary>
        /// Returns a random string where its size is within a specified range.
        /// </summary>
        /// <param name="MinSize">The inclusive lower bound of the size of the string returned.</param>
        /// <param name="MaxSize">The exclusive upper bound of the size of the string returned.</param>
        /// <returns></returns>
        public static string GenerateString(int MinSize = 40, int MaxSize = 40)
        {
            // Create the string builder with a specific capacity
            var builder = new StringBuilder(Randomizer.GenerateNumber(MinSize, MaxSize));

            // Fill the string builder
            for (var i = 0; i < builder.Capacity; i++)
            {
                builder.Append(Randomizer.AllowedChars[Randomizer.GenerateNumber(Randomizer.AllowedChars.Length - 1)]);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Guid"/> structure.
        /// </summary>
        /// <returns>A new <see cref="Guid"/> object.</returns>
        public static Guid GenerateGuid()
        {
            return Guid.NewGuid();
        }
    }
}