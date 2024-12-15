using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeRuleAnalyzer.Rules
{
    internal class PropertyRuleManager
    {
        public static void ApplyAll(PropertyDeclarationSyntax node, string filePath)
        {
            Console.WriteLine($"Analyzing property '{node.Identifier.Text}' in file {filePath}");
        }
    }
}
