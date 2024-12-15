using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeRuleAnalyzer.Rules
{
    internal static class MethodRuleManager
    {
        public static void ApplyAll(MethodDeclarationSyntax node, string filePath)
        {
            Console.WriteLine($"Analyzing method '{node.Identifier.Text}' in file {filePath}");

            ApplyMethodNameRule(node, filePath);
            ApplyParamNameRule(node, filePath);
        }

        private static void ApplyParamNameRule(MethodDeclarationSyntax node, string filePath)
        {
            foreach (var param in node.ParameterList.Parameters)
            {
                var paramName = param.Identifier.Text;

                // Check for "id" naming violation
                if (paramName.Equals("id", StringComparison.OrdinalIgnoreCase))
                {
                    ReportViolation(
                        param.Identifier,
                        filePath,
                        $"Naming style violation: Parameter '{paramName}' should be more specific (e.g., 'userId', 'productId').",
                        Logger.LogLevel.Warning
                    );
                }

                // Check for camelCase naming style violation
                if (char.IsUpper(paramName[0]))
                {
                    ReportViolation(
                        param.Identifier,
                        filePath,
                        $"Naming style violation: Parameter '{paramName}' should follow camelCase naming.",
                        Logger.LogLevel.Warning
                    );
                }
            }
        }

        private static void ApplyMethodNameRule(MethodDeclarationSyntax node, string filePath)
        {
            var methodName = node.Identifier.Text;

            // Check for PascalCase naming style violation
            if (!char.IsUpper(methodName[0]))
            {
                ReportViolation(
                    node.Identifier,
                    filePath,
                    $"Naming style violation: Method '{methodName}' is not in PascalCase.",
                    Logger.LogLevel.Warning
                );
            }
        }

        private static void ReportViolation(SyntaxToken violatingToken, string filePath, string message, Logger.LogLevel level)
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
