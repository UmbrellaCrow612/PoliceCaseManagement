using CodeRuleAnalyzer.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeRuleAnalyzer.LibRules
{
    public class ClassRule : CodeRule
    {
        public override void Analyze(SyntaxNode node)
        {
            if (node is ClassDeclarationSyntax)
            {
                Console.WriteLine($"Running ClassRule class for a class");
            }
        }
    }
}
