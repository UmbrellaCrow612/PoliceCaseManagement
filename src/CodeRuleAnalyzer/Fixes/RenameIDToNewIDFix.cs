using CodeRule.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeRuleAnalyzer.Fixes
{
    public class RenameIDToNewIDFix : CodeFix
    {
        public override SyntaxNode ApplyFix(SyntaxNode root)
        {
            var identifierNodes = root.DescendantNodes()
                .OfType<IdentifierNameSyntax>()
                .Where(id => newID.Identifier.Text.Equals("ID", StringComparison.OrdinalIgnoreCase));

            var updatedRoot = root.ReplaceNodes(
                identifierNodes,
                (oldNode, _) => oldNode.WithIdentifier(SyntaxFactory.Identifier("newID"))
            );

            return updatedRoot;
        }
    }
}
