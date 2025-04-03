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
            //Console.WriteLine($"Generating {baseName}.cs in {outputDir}...");
            string targetDir = Path.Combine(outputDir, "..", "BISAYA++");
            Directory.CreateDirectory(targetDir); // Ensure the folder exists

            string path = Path.Combine(targetDir, baseName + ".cs");
            //string path = Path.Combine(outputDir, baseName + ".cs");

            if (!Directory.Exists(targetDir))
            {
                Console.WriteLine($"Directory does not exist. Creating: {targetDir}");
                Directory.CreateDirectory(targetDir);
            }

            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                writer.WriteLine("using System;");
                writer.WriteLine("using System.Collections.Generic;");
                writer.WriteLine();
                writer.WriteLine($"namespace BISAYA__");
                writer.WriteLine("{");
                writer.WriteLine($"    internal abstract class {baseName}");
                writer.WriteLine("    {");

                // Generate the visitor interface
                defineVisitor(writer, baseName, types);

                // Generate AST classes
                foreach (string type in types)
                {
                    string[] parts = type.Split(": ");
                    string className = parts[0].Trim();
                    string fields = parts[1].Trim();
                    defineType(writer, baseName, className, fields);
                }

                // The base accept() method
                writer.WriteLine();
                writer.WriteLine("        public abstract R Accept<R>(IVisitor<R> visitor);");

                writer.WriteLine("    }");
                writer.WriteLine("}");
            }

            //Console.WriteLine($"Successfully generated {baseName}.cs!");
        }

        private static void defineVisitor(StreamWriter writer, string baseName, List<string> types)
        {
            writer.WriteLine("        public interface IVisitor<R>");
            writer.WriteLine("        {");

            foreach (string type in types)
            {
                string typeName = type.Split(':')[0].Trim();
                writer.WriteLine($"            R Visit{typeName}{baseName}({typeName} {baseName.ToLower()});");
            }

            writer.WriteLine("        }");
            writer.WriteLine();
        }
  

        private static void defineType(StreamWriter writer, string baseName, string className, string fieldList)
        {
            writer.WriteLine($"        internal class {className} : {baseName}");
            writer.WriteLine("        {");

            // Parse fields
            string[] fields = fieldList.Split(", ");

            // Constructor
            writer.Write($"            internal {className}(");
            writer.Write(string.Join(", ", fields));
            writer.WriteLine(")");
            writer.WriteLine("            {");

            foreach (string field in fields)
            {
                string[] parts = field.Split(" ");
                string type = parts[0];
                string name = parts[1];

                writer.WriteLine($"                this.{name} = {name};");
            }

            writer.WriteLine("            }");
            writer.WriteLine();

            // Visitor pattern: accept() method (placed above fields)
            writer.WriteLine("            public override R Accept<R>(IVisitor<R> visitor)");
            writer.WriteLine("            {");
            writer.WriteLine($"                return visitor.Visit{className}{baseName}(this);");
            writer.WriteLine("            }");
            writer.WriteLine();

            // Fields
            foreach (string field in fields)
            {
                writer.WriteLine($"            internal readonly {field};");
            }

            writer.WriteLine("        }");
        }
    }
}
