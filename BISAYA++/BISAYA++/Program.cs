// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BISAYA__
{
    public class BISAYA
    {
        static bool hadError = false;
        public static void Main(string[] args)
        {
            //System.Console.WriteLine("Hello, World!");
            if(args.Length > 1)
            {
                System.Console.WriteLine("Usage: BISAYA [script]");
                Environment.Exit(64);
            } else if(args.Length == 1)
            {
                RunFile(args[0]);
            } else
            {
                RunPrompt();
            }
        }

        private static void RunFile(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);
            Run(Encoding.Default.GetString(bytes));

            // Indicate an error in the exit code.
            if (hadError) Environment.Exit(65);
        }

        private static void RunPrompt()
        {
            while (true)
            {
                Console.Write("> ");
                string line = Console.ReadLine();
                if (line == null) break;
                Run(line);

                hadError = false;
            }
        }

        private static void Run(string source)
        {
            Scanner scanner = new Scanner(source);
            List<Token> tokens = scanner.scanTokens();
            // For now, just print the tokens.
            foreach (Token token in tokens)
            {
                Console.WriteLine(token);
            }
        }

        public static void Error(int line, string message)
        {
            Report(line, "", message);
        }
        private static void Report(int line, string where,
        string message)
        {
            Console.Error.WriteLine($"[line {line}] Error{where}: {message}");
            hadError = true;
        }



    }
}
