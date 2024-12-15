using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeRuleAnalyzer.Rules
{
    internal class PropertyRules
    {
        public static void Apply(PropertyDeclarationSyntax node, string filePath)
        {
            Console.WriteLine($"Analyzing property '{node.Identifier.Text}' in file {filePath}");
        }
    }
}
