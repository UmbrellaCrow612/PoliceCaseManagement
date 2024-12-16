using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using CodeRuleAnalyzer.Rules;
using CodeRuleAnalyzer.Security;
using CodeRuleAnalyzer.Complexity;

namespace CodeRuleAnalyzer
{
    internal class Analyzers
    {
        public static void AnalyzeNode(SyntaxNode node, string filePath)
        {
            switch (node)
            {
                case MethodDeclarationSyntax methodNode:
                    AnalyzeMethod(methodNode, filePath);
                    break;

                case VariableDeclarationSyntax variableNode:
                    AnalyzeVariable(variableNode, filePath);
                    break;

                case ClassDeclarationSyntax classNode:
                    AnalyzeClass(classNode, filePath);
                    break;

                case PropertyDeclarationSyntax propertyNode:
                    AnalyzeProperty(propertyNode, filePath);
                    break;

                case FieldDeclarationSyntax fieldNode:
                    AnalyzeField(fieldNode, filePath);
                    break;

                case IfStatementSyntax ifStatementNode:
                    AnalyzeIfStatement(ifStatementNode, filePath);
                    break;

                default:
                    AnalyzeUnknownNode(node, filePath);
                    break;
            }
        }

        private static void AnalyzeMethod(MethodDeclarationSyntax methodNode, string filePath)
        {
            MethodRuleManager.ApplyAll(methodNode, filePath);
        }

        private static void AnalyzeVariable(VariableDeclarationSyntax variableNode, string filePath)
        {
            VariableRuleManager.ApplyAll(variableNode, filePath);
            VariableSecurityManager.ApplyAll(variableNode, filePath);
            VariableComplexityManager.ApplyAll(variableNode, filePath);
        }

        private static void AnalyzeClass(ClassDeclarationSyntax classNode, string filePath)
        {
            ClassRuleManager.ApplyAll(classNode, filePath);
        }

        private static void AnalyzeProperty(PropertyDeclarationSyntax propertyNode, string filePath)
        {
            PropertyRuleManager.ApplyAll(propertyNode, filePath);
        }

        private static void AnalyzeField(FieldDeclarationSyntax fieldNode, string filePath)
        {
            FieldRuleManager.ApplyAll(fieldNode, filePath);
        }

        private static void AnalyzeIfStatement(IfStatementSyntax ifStatementNode, string filePath)
        {
            IfStatementRuleManager.ApplyAll(ifStatementNode, filePath);
        }

        private static void AnalyzeUnknownNode(SyntaxNode node, string filePath)
        {

        }
    }
}
