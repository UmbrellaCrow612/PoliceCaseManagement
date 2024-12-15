using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeRuleAnalyzer.Rules
{
    internal static class MethodRuleManager
    {
        public static void ApplyAll(MethodDeclarationSyntax node, string filePath)
        {
            Console.WriteLine($"Analyzing method '{node.Identifier.Text}' in file {filePath}");

            ApplyMethodNameRule(node);
            ApplyParamNameRule(node);
        }

        private static void ApplyParamNameRule(MethodDeclarationSyntax node)
        {
            foreach (var param in node.ParameterList.Parameters)
            {
                // call code to check param naming and other stuff
            }
        }

        private static void ApplyMethodNameRule(MethodDeclarationSyntax node)
        {

        }
    }
}
