using Microsoft.CodeAnalysis;

namespace CodeRule.Core
{
    public abstract class CodeRule
    {
        public abstract void Analyze(SyntaxNode node);
    }
}
