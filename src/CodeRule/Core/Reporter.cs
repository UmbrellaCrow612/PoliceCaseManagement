using Microsoft.CodeAnalysis;

namespace CodeRule.Core
{
    public static class Reporter
    {
        public static void ReportViolation(SyntaxToken violatingToken, string filePath, string message, Logger.LogLevel level)
        {
            // Extract line and column information from the token
            var lineSpan = violatingToken.GetLocation().GetLineSpan();
            var lineNumber = lineSpan.StartLinePosition.Line + 1; // Line numbers are 0-based
            var columnNumber = lineSpan.StartLinePosition.Character + 1; // Columns are 0-based

            // Highlight the specific text of the violating token
            var highlightedCode = violatingToken.Text;

            // Log the violation with detailed information
            Logger.Log(
                $"{message} (File: {filePath}, Line: {lineNumber}, Column: {columnNumber}, Code: '{highlightedCode}')",
                level
            );
        }
    }
}
