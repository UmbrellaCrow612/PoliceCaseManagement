using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
                        AddViolation(
                          filePath,
                          parameter.Identifier,
                          "Avoid using 'id' as a parameter name. Use a more descriptive name.",
                          "Warning"
                      );
                    }
                }
            }
        }
    }

}