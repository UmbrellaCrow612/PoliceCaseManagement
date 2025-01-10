using System.Reflection;

namespace CodeRule.Core
{
    internal class ReflectionHelper
    {
        public static List<CodeRule> FindAllCodeRules()
        {
            var baseType = typeof(CodeRule);
            var coreAssembly = baseType.Assembly;

            // Get all loaded assemblies that reference CodeRule.Core
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => ReferencesAssembly(a, coreAssembly));

            return assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && baseType.IsAssignableFrom(t))
                .Select(t => (CodeRule)Activator.CreateInstance(t)!)
                .ToList();
        }

        public static List<CodeFix> FindAllCodeFixes()
        {
            var baseType = typeof(CodeFix);
            var coreAssembly = baseType.Assembly;

            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => ReferencesAssembly(a, coreAssembly));

            return assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && baseType.IsAssignableFrom(t))
                .Select(t => (CodeFix)Activator.CreateInstance(t)!)
                .ToList();
        }

        private static bool ReferencesAssembly(Assembly assembly, Assembly targetAssembly)
        {
            // Check if the assembly references the target assembly
            return assembly.GetReferencedAssemblies()
                .Any(referencedAssembly => referencedAssembly.FullName == targetAssembly.FullName);
        }
    }
}
