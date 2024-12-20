using CodeRule.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeRuleAnalyzer.Fixes.Methods.Paramas
{
    internal class IdNameFix : CodeFix
    {
        public override SyntaxNode ApplyFix(SyntaxNode root)
        {
            var updatedRoot = root.ReplaceNodes(
                root.DescendantNodes().OfType<MethodDeclarationSyntax>(),
                (originalNode, rewrittenNode) => FixMethodParameters(rewrittenNode)
            );

            return updatedRoot;
        }

        private MethodDeclarationSyntax FixMethodParameters(MethodDeclarationSyntax method)
        {
            var updatedParameterList = method.ParameterList.Parameters
                .Select(param => RenameIdParameter(param))
                .ToList();

            var newParameterList = SyntaxFactory.ParameterList(
                SyntaxFactory.SeparatedList(updatedParameterList)
            );

            return method.WithParameterList(newParameterList);
        }

        private ParameterSyntax RenameIdParameter(ParameterSyntax parameter)
        {
            if (parameter.Identifier.Text == "ID")
            {
                return parameter.WithIdentifier(SyntaxFactory.Identifier("newID"));
            }

            return parameter;
        }
    }
}
