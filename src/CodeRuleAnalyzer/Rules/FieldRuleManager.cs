﻿using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeRuleAnalyzer.Rules
{
    internal class FieldRuleManager
    {
        public static void ApplyAll(FieldDeclarationSyntax node, string filePath)
        {
            Console.WriteLine($"Analyzing field in file {filePath}");
        }
    }
}