using System;
using System.Diagnostics;

namespace Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            CommandLineArguments options = CommandLineArguments.Parse(args);
            using (Game1 game = new Game1(options.Host, options.Port))
            {
                game.Run();
            }
        }

    }
}

