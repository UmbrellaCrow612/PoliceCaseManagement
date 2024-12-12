using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Style.Core;

namespace Style.Styles
{
    public class CSharpStyleChecker : StyleBase
    {
        public void CheckFile(string filePath)
        {
            string code = System.IO.File.ReadAllText(filePath);
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
            var root = syntaxTree.GetRoot();

            // Traverse all nodes and check against rules
            foreach (var node in root.DescendantNodes())
            {
                foreach (var rule in _rules)
                {
                    var violation = rule.CheckViolation(node);
                    ProcessViolation(violation);
                }
            }
        }

        public void CheckDirectory(string directoryPath)
        {
            var csFiles = System.IO.Directory.GetFiles(directoryPath, "*.cs", System.IO.SearchOption.AllDirectories);
            foreach (var file in csFiles)
            {
                CheckFile(file);
            }
        }

        public void PrintViolations()
        {
            foreach (var violation in _violations)
            {
                ConsoleColor color = violation.Severity switch
                {
                    ViolationSeverity.Error => ConsoleColor.Red,
                    ViolationSeverity.Warning => ConsoleColor.Yellow,
                    ViolationSeverity.Info => ConsoleColor.Blue,
                    _ => ConsoleColor.White
                };

                Console.ForegroundColor = color;
                Console.WriteLine(violation);
                Console.ResetColor();
            }
        }
    }
}
