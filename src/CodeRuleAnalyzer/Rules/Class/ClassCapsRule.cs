using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;

namespace CodeRuleAnalyzer.Rules.Class
{
    internal class ClassCapsRule : CodeRule.Core.CodeRule
    {
        public override void Analyze(SyntaxNode node)
        {
            if (node is ClassDeclarationSyntax)
            {
                Console.WriteLine("ClassCapsRule rule ran");
            }
        }
    }
}
