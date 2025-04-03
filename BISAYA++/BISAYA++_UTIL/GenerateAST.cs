using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tool
{
    internal class GenerateAST
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: generate_ast <output directory>");
                Environment.Exit(64);
            }

            string outputDir = args[0];

            defineAst(outputDir, "Expr", new List<string>
            {
                "Binary : Expr left, Token operatorToken, Expr right",
                "Grouping : Expr expression",
                "Literal : object value",
                "Unary : Token operatorToken, Expr right"
            });
        }

        private static void defineAst(string outputDir, string baseName, List<string> types)
        {
            string path = Path.Combine(outputDir, baseName + ".cs");

            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                writer.WriteLine("using System;");
                writer.WriteLine("using System.Collections.Generic;");
                writer.WriteLine();
                writer.WriteLine($"namespace BISAYA__");
                writer.WriteLine("{");
                writer.WriteLine($"    internal abstract class {baseName}");
                writer.WriteLine("    {");

                writer.WriteLine("    }");
                writer.WriteLine("}");
            }
        }
    }
}
