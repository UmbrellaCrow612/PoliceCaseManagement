using CodeRuleAnalyzer.Core;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeRuleAnalyzer.LibRules
{
    public class ClassRule : CodeRule
    {
        public void Analyze(ClassDeclarationSyntax node)
        {
            Console.WriteLine($"ClassRule: Analyzing class '{node.Identifier.Text}'");
        }
    }
}
