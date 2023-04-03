using System.Security.Cryptography;

namespace Engine.Core.Utils
{
    public static class RandomGeneration
    {
        #region Fields

        /// <summary>
        /// Used to generate random numbers.
        /// </summary>
        private static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();

        #endregion Fields

        #region Public Methods

        /// <summary>
        /// Generates a random double between a given range.
        /// </summary>
        /// <remarks>Extracted and modified from http://scottlilly.com/create-better-random-numbers-in-c/</remarks>
        /// <param name="lowerLt">Lower limit of the range.</param>
        /// <param name="higherLt">Higher limit of the range.</param>
        /// <returns>A random number.</returns>
        public static double Random(int lowerLt, int higherLt)
        {
            byte[] randomNumber = new byte[1];

            _generator.GetBytes(randomNumber);

            double asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);

            // We are using Math.Max, and substracting 0.00000000001, to ensure "multiplier" will
            // always be between 0.0 and .99999999999 Otherwise, it's possible for it to be "1",
            // which causes problems in our rounding.
            double multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d);

            //Get the range
            int range = higherLt - lowerLt;

            double randomValueInRange = (multiplier * range);

            return (lowerLt + randomValueInRange);
        }

        #endregion Public Methods
    }
}