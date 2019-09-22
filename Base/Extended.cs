using System;

namespace Engine
{
    namespace Base
    {
        public static class Extended
        {
            /// <summary>Reverses a String.</summary>
            /// <param name="inputStr">String to be reversed.</param>
            /// <returns>The reversed string</returns>
            public static String Reverse(this string inputStr)
            {
                if (string.IsNullOrEmpty(inputStr))
                {
                    throw new ArgumentException("String cannot be empty!", nameof(inputStr));
                }

                char[] arr = inputStr.ToCharArray();
                Array.Reverse(arr);
                return new string(arr);
            }

            /// <summary>
            /// Adds a specified amount of tabulations before or after a certain string.
            /// </summary>
            /// <param name="str">Base String</param>
            /// <param name="amount">Number of tabulations</param>
            /// <param name="before">Optional. Set to FALSE to add tabulations after the string. Default value is TRUE</param>
            /// <returns>The formatted String</returns>
            public static string AddTabs(this string str, int amount, bool before = true)
            {
                if (string.IsNullOrEmpty(str))
                {
                    throw new ArgumentException("String cannot be empty!", nameof(str));
                }

                string tabs = "";
                for (int i = 0; i < amount; tabs += General.TAB_SEPARATOR, i++) ; //Adds a tabulation every iteration.
                if (before) return tabs + str; //Returns the string with tabulations before it.
                else return str + tabs; //Returns the string with tabulations after it.
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
        }
    }
}