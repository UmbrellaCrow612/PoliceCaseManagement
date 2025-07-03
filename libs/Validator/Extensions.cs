using Microsoft.Extensions.DependencyInjection;

namespace Validator
{
    /// <summary>
    /// Custom extension methods to add stuff to the program services
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Call this on program startup before build to add all defined <see cref="Validator{T}"/> into the DI pipeline
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                var validatorTypes =
                    assembly
                        .GetTypes()
                        .Where(t => t.IsClass && !t.IsAbstract && InheritsFromValidator(t))
                        .ToList();

                foreach (var type in validatorTypes)
                {
                    services.AddSingleton(type);
                }
            }

            return services;
        }

        private static bool InheritsFromValidator(Type type)
        {
            var current = type.BaseType;
            while (current != null)
            {
                if (current.IsGenericType &&
                    current.GetGenericTypeDefinition() == typeof(Validator<>))
                {
                    return true;
                }
                current = current.BaseType;
            }
            return false;
        }
    }
}
