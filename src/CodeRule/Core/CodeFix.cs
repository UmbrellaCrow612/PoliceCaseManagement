using Microsoft.CodeAnalysis;

namespace CodeRule.Core
{
    public abstract class CodeFix
    {
        public virtual string GetCodeFixName()
        {
            return this.GetType().Name;
        }

        public abstract SyntaxNode ApplyFix(SyntaxNode root);
    }
}
