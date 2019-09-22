using System;
using System.Security.Cryptography;

namespace Engine
{
    namespace Base
    {
        /// <summary>
        /// This class provides basic functions and methods that are used by other classes in this library. It can be used by itself.
        /// </summary>
        public static class General
        {
            /// <summary>
            /// Enviroment Line Separator
            /// </summary>
            public static readonly string LINE_SEPARATOR = Environment.NewLine;

            /// <summary>
            /// Tabulation Character
            /// </summary>
            public static readonly string TAB_SEPARATOR = "\t";

            /// <summary>
            /// OS's path separator.
            /// </summary>
            public static readonly string FILE_SEPARATOR = System.IO.Path.DirectorySeparatorChar.ToString();

            /// <summary>
            /// Used to generate random numbers.
            /// </summary>
            private static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();

            /// <summary>
            /// Generates a random between a given range.
            /// </summary>
            /// <remarks>Extracted from http://scottlilly.com/create-better-random-numbers-in-c/ </remarks>
            /// <param name="lowerLt">Lower limit of the range.</param>
            /// <param name="higherLt">Higher limit of the range.</param>
            /// <returns>A random number.</returns>
            public static double Random(int lowerLt, int higherLt)
            {
                byte[] randomNumber = new byte[1];

                _generator.GetBytes(randomNumber);

                double asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);

                // We are using Math.Max, and substracting 0.00000000001,
                // to ensure "multiplier" will always be between 0.0 and .99999999999
                // Otherwise, it's possible for it to be "1", which causes problems in our rounding.
                double multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d);

                // We need to add one to the range, to allow for the rounding done with Math.Floor
                int range = higherLt - lowerLt + 1;

                double randomValueInRange = (multiplier * range);

                return (lowerLt + randomValueInRange);
            }

            /// <summary>
            /// Gets the percent a value is of a total value.
            /// </summary>
            /// <param name="total">The total value</param>
            /// <param name="value">The value to calculate the percent of.</param>
            /// <returns></returns>
            public static double getPercent(double total, double value)
            {
                return (value / total) * 100;
            }
        }
    }
}