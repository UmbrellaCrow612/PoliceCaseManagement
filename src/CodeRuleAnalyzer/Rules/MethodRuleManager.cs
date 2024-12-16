using CodeRuleAnalyzer.Helpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeRuleAnalyzer.Rules
{
    internal static class MethodRuleManager
    {
        public static void ApplyAll(MethodDeclarationSyntax node, string filePath)
        {
            ApplyMethodNameRule(node, filePath);
            ApplyParamNameRule(node, filePath);
            ApplyMethodReturnTypeRule(node, filePath);
        }

        private static void ApplyParamNameRule(MethodDeclarationSyntax node, string filePath)
        {
            foreach (var param in node.ParameterList.Parameters)
            {
                var paramName = param.Identifier.Text;

                // Check for "id" naming violation
                if (paramName.Equals("id", StringComparison.OrdinalIgnoreCase))
                {
                    Reporter.ReportViolation(
                        param.Identifier,
                        filePath,
                        $"Naming style violation: Parameter '{paramName}' should be more specific (e.g., 'userId', 'productId').",
                        Logger.LogLevel.Warning
                    );
                }

                // Check for camelCase naming style violation
                if (char.IsUpper(paramName[0]))
                {
                    Reporter.ReportViolation(
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
                Reporter.ReportViolation(
                    node.Identifier,
                    filePath,
                    $"Naming style violation: Method '{methodName}' is not in PascalCase.",
                    Logger.LogLevel.Warning
                );
            }
        }

        private static void ApplyMethodReturnTypeRule(MethodDeclarationSyntax node, string filePath)
        {
           
        }   
    }
}
