using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tubular.Http;

namespace Tubular
{
    class Program
    {
        static LoggerClass log;

        // Main method for all of tubular
        static void Main(string[] args)
        {
            log = new LoggerClass("main");

            /* 
             * *** Parse command line arguments ***
             */

            if (args.Length != 3)
            {
                if (args.Length == 1 && args[0] == "help")
                {
                    // If the help command was used then print the usage and exit
                    PrintHelp();
                    Environment.Exit(0);
                }
                else
                {
                    // Invalid number of arguments 
                    InvalidArgs();
                }
            }

            // Variables for each argument
            bool isApp = false;
            string filePath = "";
            int port = -1;

            // Get mode argument
            switch (args[0].ToLower())
            {
                case "app": isApp = true; break;
                case "file": isApp = false; break;
                default: InvalidArgs(); break;
            }

            // Get file path argument
            filePath = args[1];

            // Get port argument and make sure it's a positive number
            if(!int.TryParse(args[2], out port) || port <= 0)
                InvalidArgs();

            /*
             * *** Run Tubular ***
             */

            // Start tubular
            log.LogInfo("Starting Tubular...");
            Server server = new Server(isApp, filePath, port);
            server.Start();

            // Wait for key press
            log.LogInfo("Tubular started, press any key to stop...");
            Console.ReadKey(true);

            // Stop tubular
            server.Stop();
            log.LogInfo("Tubular stoped.");
        }

        // Called for any invalid argument
        static void InvalidArgs()
        {
            log.LogError("Invalid command line arguments. For help type 'tubular help'.");
            Environment.Exit(0); // The program can't continue if there's an invalid argument
        }

        // Prints the command line usage
        static void PrintHelp()
        {
            Console.WriteLine("Usage: tubular <mode> <file path> <port>");
            Console.WriteLine("    Mode:      'app' or 'file'");
            Console.WriteLine("    File Path: The path to your app or file. (eg. 'apps/app.exe')");
            Console.WriteLine("    Port:      The port to run the server on.");
            Console.WriteLine();
            Console.Write("For more help visit ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("https://github.com/YumProjects/Tubular/wiki");
            Console.ResetColor(); // Reset to the original command line color for exit
        }
    }
}