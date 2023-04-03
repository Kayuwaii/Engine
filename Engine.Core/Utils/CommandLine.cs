using Engine.Base;

namespace Engine
{
    namespace Advanced
    {
        public static class CommandLine
        {
            #region Fields

            /// <summary>
            /// Color used for error messages.
            /// </summary>
            private static ConsoleColor ERROR_COLOR = ConsoleColor.Red;

            /// <summary>
            /// Color used for input text.
            /// </summary>
            private static ConsoleColor INPUT_COLOR = ConsoleColor.Green;

            #endregion Fields

            #region Public Methods

            /// <summary>
            /// Displays an error message.
            /// </summary>
            /// <param name="msg">Message content</param>
            public static void ErrorMsg(string msg)
            {
                Console.ForegroundColor = ERROR_COLOR;
                Console.WriteLine(msg);
                Console.ResetColor();
            }

            /// <summary>
            /// Reads a input from console and parses it as a Double if it's not a number, it
            /// repeats until a number is provided.
            /// </summary>
            /// <param name="promt">Optional. Promt before text input.</param>
            /// <returns>The value writen by the user.</returns>
            public static Double ReadNum(string promt = "")
            {
                do
                {
                    Console.Write(promt);
                    String input = UserInput();
                    Double num;
                    if (Double.TryParse(input.Replace(".", ","), out num)) return num;
                    Console.ForegroundColor = ConsoleColor.Red;
                    ErrorMsg("ERROR. Please Input A Number");
                    Console.ResetColor();
                } while (true);
            }

            /// <summary>
            /// Sets the color used on error messages
            /// </summary>
            /// <param name="choice">Optional. Desired color, leave blank for defaulr(Red)</param>
            public static void SetErrorColor(ConsoleColor choice = ConsoleColor.Red)
            {
                ERROR_COLOR = choice;
            }

            /// <summary>
            /// Sets the color used on input text.
            /// </summary>
            /// <param name="choice">Optional. Desired color, leave blank for defaulr(Green)</param>
            public static void SetInputColor(ConsoleColor choice = ConsoleColor.Green)
            {
                INPUT_COLOR = choice;
            }

            /// <summary>
            /// Displays a given set of strings in a "menu" format and reads teh selected option.
            /// </summary>
            /// <param name="options">Every option for the menu.</param>
            /// <param name="numTabs">Optional. Amount of extra tabulations.</param>
            /// <returns>Selected option.</returns>
            public static int ShowMenu(string[] options, int numTabs = 0)
            {
                Console.WriteLine("SELECT AN OPTION:".AddTabs(numTabs) + General.LINE_SEPARATOR);
                for (int i = 1; i <= options.Length; i++)
                {
                    Console.WriteLine((i + ". " + options[i - 1]).AddTabs(numTabs + 1) + General.LINE_SEPARATOR);
                }
                Console.WriteLine((0 + ". " + "EXIT").AddTabs(numTabs + 1) + General.LINE_SEPARATOR);
                do
                {
                    int sel = (int)ReadNum("Input your selection: ".AddTabs(numTabs));
                    if (sel >= 0 && sel <= options.Length) return sel;
                    else
                    {
                        ErrorMsg("ERROR. Enter a valid number");
                    }
                } while (true);
            }

            /// <summary>
            /// Retrieves user input. Applies color to input text.
            /// </summary>
            /// <returns>The string the user wrote.</returns>
            public static string UserInput()
            {
                Console.ForegroundColor = INPUT_COLOR;
                string inpStr = Console.ReadLine();
                Console.ResetColor();
                return inpStr;
            }

            #endregion Public Methods
        }
    }
}