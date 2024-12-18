using Microsoft.CodeAnalysis;

namespace CodeRule.Core
{
    public static class Reporter
    {
        public static void ReportViolation(SyntaxToken violatingToken, string filePath, string message, Logger.LogLevel level, string? csvFilePath = null)
        {
            var lineSpan = violatingToken.GetLocation().GetLineSpan();
            var lineNumber = lineSpan.StartLinePosition.Line + 1; 
            var columnNumber = lineSpan.StartLinePosition.Character + 1; 

            var highlightedCode = violatingToken.Text;

            Logger.Log(
                $"{message} (File: {filePath}, Line: {lineNumber}, Column: {columnNumber}, Code: '{highlightedCode}')",
                level
            );

            if (!string.IsNullOrWhiteSpace(csvFilePath))
            {
                WriteViolationToCsv(csvFilePath, filePath, violatingToken.Text, message, level.ToString());
            }
        }

        private static void WriteViolationToCsv(string csvFilePath, string filePath, string parameterName, string violationDescription, string severity)
        {
            string violation = $"{filePath},{parameterName},\"{violationDescription}\",{severity}";

            using StreamWriter writer = new(csvFilePath, append: true);
            writer.WriteLine(violation);
        }
    }
}
