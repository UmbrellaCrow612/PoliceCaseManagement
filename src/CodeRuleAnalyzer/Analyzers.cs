using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using CodeRuleAnalyzer.Rules;

namespace CodeRuleAnalyzer
{
    internal class Analyzers
    {
        public static void AnalyzeNodeWithRules(SyntaxNode node, string filePath)
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
            MethodRules.Apply(methodNode, filePath);
        }

        private static void AnalyzeVariable(VariableDeclarationSyntax variableNode, string filePath)
        {
            VariableRules.Apply(variableNode, filePath);
        }

        private static void AnalyzeClass(ClassDeclarationSyntax classNode, string filePath)
        {
            ClassRules.Apply(classNode, filePath);
        }

        private static void AnalyzeProperty(PropertyDeclarationSyntax propertyNode, string filePath)
        {
            PropertyRules.Apply(propertyNode, filePath);
        }

        private static void AnalyzeField(FieldDeclarationSyntax fieldNode, string filePath)
        {
            FieldRules.Apply(fieldNode, filePath);
        }

        private static void AnalyzeIfStatement(IfStatementSyntax ifStatementNode, string filePath)
        {
            IfStatementRules.Apply(ifStatementNode, filePath);
        }

        private static void AnalyzeUnknownNode(SyntaxNode node, string filePath)
        {

        }
    }
}
