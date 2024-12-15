using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeRuleAnalyzer.Rules
{
    internal class ClassRuleManager
    {
        public static void ApplyAll(ClassDeclarationSyntax node, string filePath)
        {
            Console.WriteLine($"Analyzing class '{node.Identifier.Text}' in file {filePath}");
        }
    }
}
