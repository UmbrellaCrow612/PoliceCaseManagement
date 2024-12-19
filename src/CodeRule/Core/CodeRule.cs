using Microsoft.CodeAnalysis;

namespace CodeRule.Core
{
    public abstract class CodeRule
    {
        private List<Violation> Violations { get; } = [];

        public abstract void Analyze(SyntaxNode rootNode, string filePath);

        public virtual string GetRuleClassName()
        {
            return this.GetType().Name;
        }

        public IReadOnlyList<Violation> GetViolations()
        {
            return Violations.AsReadOnly();
        }

        public void ClearViolations()
        {
            Violations.Clear();
        }

        protected void AddViolation(string filePath, SyntaxToken violatingToken, string description, string severity)
        {
            (string lineNumber, string columnNumber) = GetTokenPosition(violatingToken);

            Violations.Add(new Violation
            {
                FilePath = filePath,
                ViolatingToken = violatingToken,
                LineNumber = lineNumber, 
                ColumnNumber = columnNumber, 
                ViolationDescription = description,
                Severity = severity
            });
        }

        private static (string lineNumber, string columnNumber) GetTokenPosition(SyntaxToken violatingToken)
        {
            var lineSpan = violatingToken.GetLocation().GetLineSpan();
            var lineNumber = (lineSpan.StartLinePosition.Line + 1).ToString();
            var columnNumber = (lineSpan.StartLinePosition.Character + 1).ToString();
            return (lineNumber, columnNumber);
        }
    }
}
