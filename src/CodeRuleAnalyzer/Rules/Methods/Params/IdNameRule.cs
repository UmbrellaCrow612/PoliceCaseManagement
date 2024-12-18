using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeRule.Core;

namespace CodeRuleAnalyzer.Rules.Methods.Params
{
    internal class IdNameRule : CodeRule.Core.CodeRule
    {
        public override void Analyze(SyntaxNode node, string filePath)
        {
            if (node is MethodDeclarationSyntax methodDeclaration)
            {
                foreach (var parameter in methodDeclaration.ParameterList.Parameters)
                {
                    if (parameter.Identifier.Text.Equals("id", StringComparison.OrdinalIgnoreCase))
                    {
                        Reporter.ReportViolation(
                            parameter.Identifier,
                            filePath,
                            $"Parameter '{parameter.Identifier.Text}' is too generic. Consider using a more descriptive name like 'userId', 'productId', etc.",
                            Logger.LogLevel.Warning,
                            "A-Method.csv"
                        );
                    }
                }
            }
        }
    }

}