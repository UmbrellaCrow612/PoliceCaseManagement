using Microsoft.CodeAnalysis;

namespace CodeRule.Core
{
    public abstract class CodeFix
    {
        public abstract SyntaxNode ApplyFix(SyntaxNode root);
    }
}
