using System.Reflection;

namespace CodeRuleAnalyzer.Core
{
    internal class ReflectionHelper
    {
        public static List<CodeRule> FindAllCodeRules()
        {
            var baseType = typeof(CodeRule);

            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && baseType.IsAssignableFrom(t))
                .Select(t => (CodeRule)Activator.CreateInstance(t))
                .ToList();
        }
    }
}
