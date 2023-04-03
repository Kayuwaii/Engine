namespace Engine
{
    namespace Base
    {
        /// <summary>
        /// This class provides basic functions and methods that are used by other classes in this
        /// library. It can be used by itself.
        /// </summary>
        public static class General
        {
            #region Fields

            /// <summary>
            /// OS's path separator.
            /// </summary>
            public static readonly string FILE_SEPARATOR = System.IO.Path.DirectorySeparatorChar.ToString();

            /// <summary>
            /// Enviroment Line Separator
            /// </summary>
            public static readonly string LINE_SEPARATOR = Environment.NewLine;

            /// <summary>
            /// Tabulation Character
            /// </summary>
            public static readonly string TAB_SEPARATOR = "\t";

            #endregion Fields

            #region Public Methods

            /// <summary>
            /// Gets the percent a value is of a total value.
            /// </summary>
            /// <param name="total">The total value</param>
            /// <param name="value">The value to calculate the percent of.</param>
            /// <returns></returns>
            public static double getPercent(double total, double value)
            {
                return (value / total) * 100d;
            }

            /// <summary>
            /// Check if an integer is odd or even.
            /// </summary>
            /// <param name="value">The number to check.</param>
            /// <returns>True if is even.</returns>
            public static bool isEven(this int value)
            {
                return (value & 1) == 0; //Bitwise AND. Checking lowest order bit. If it's 1 it's odd.
            }

            #endregion Public Methods
        }
    }
}