using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeRuleAnalyzer.Core
{
    internal class CodeRunner
    {
        public static void Run()
        {
            var classNode = SyntaxFactory.ClassDeclaration("MyClass");

            var nodes = new List<SyntaxNode> { classNode };

            // Find and execute all rules
            var rules = ReflectionHelper.FindAllCodeRules();

            foreach (var rule in rules)
            {
                Console.WriteLine($"Running {rule.GetType().Name}...");
                foreach (var node in nodes)
                {
                    rule.Analyze(node); // Call base Analyze, which dispatches to the right overload
                }
            }
        }
    }
}
