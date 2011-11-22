using System;

namespace SodukuWinphone
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SudokuGame game = new SudokuGame())
            {
                game.Run();
            }
        }
    }
#endif
}

