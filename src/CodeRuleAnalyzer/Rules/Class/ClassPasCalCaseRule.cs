using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeRuleAnalyzer.Rules.Class
{
    internal class ClassPasCalCaseRule : CodeRule.Core.CodeRule
    {
        public override void Analyze(SyntaxNode node)
        {
            if(node is ClassDeclarationSyntax)
            {
                Console.WriteLine("ClassPasCalCaseRule rule ran");
            }
        }
    }
}
