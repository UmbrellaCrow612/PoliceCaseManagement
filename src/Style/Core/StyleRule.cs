using Microsoft.CodeAnalysis;

namespace Style.Core
{
    public class StyleRule(string name, Func<SyntaxNode, StyleViolation?> checkViolation)
    {
        public string Name { get; } = name;
        public Func<SyntaxNode, StyleViolation?> CheckViolation { get; } = checkViolation;
    }

}
