using Microsoft.CodeAnalysis;

namespace Style.Core
{
    public class StyleViolation(
        string ruleName,
        string message,
        ViolationSeverity severity = ViolationSeverity.Warning,
        SyntaxNode? affectedNode = null)
    {
        public string RuleName { get; } = ruleName;
        public string Message { get; } = message;
        public ViolationSeverity Severity { get; } = severity;
        public SyntaxNode? AffectedNode { get; } = affectedNode;

        public override string ToString()
        {
            return $"[{Severity}] {RuleName}: {Message}";
        }
    }

}