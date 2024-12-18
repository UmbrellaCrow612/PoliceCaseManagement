using Microsoft.CodeAnalysis;

namespace CodeRuleAnalyzer.Core
{
    public abstract class CodeRule
    {
        public abstract void Analyze(SyntaxNode node);
    }
}
