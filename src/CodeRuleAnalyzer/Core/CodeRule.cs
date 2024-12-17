using Microsoft.CodeAnalysis;
using System.Reflection;

namespace CodeRuleAnalyzer.Core
{
    public abstract class CodeRule
    {
        // Public entry point: It dispatches to the correct overload
        public void Analyze(SyntaxNode node)
        {
            DispatchAnalyze(node);
        }

        // Internal method to dispatch to the correct user-defined Analyze overload
        private void DispatchAnalyze(SyntaxNode node)
        {
            // Get all "Analyze" methods in the derived class that take 1 parameter
            var method = this.GetType()
                             .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                             .FirstOrDefault(m =>
                                 m.Name == "Analyze" &&
                                 m.GetParameters().Length == 1 &&
                                 m.GetParameters()[0].ParameterType.IsInstanceOfType(node) &&
                                 m.DeclaringType != typeof(CodeRule)); // Exclude base class

            if (method != null)
            {
                method.Invoke(this, new object[] { node });
            }
            else
            {
                // Optional: Handle no overload found
                Console.WriteLine($"No specific Analyze method for node type: {node.GetType().Name}");
            }
        }
    }
}
