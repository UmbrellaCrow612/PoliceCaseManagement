using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Events.Core
{
    public static class Extensions 
    {
        /// <summary>
        /// Compile time basic check to make sure <see cref="IDenormalizedEntity"/> have at least defined a <see cref="DenormalisedEventConsumer"/> for the given entity - dose not check
        /// if they actually update the <see cref="DenormalizedFieldAttribute"/> themselves the analyzer will do that
        /// </summary>
        public static IServiceCollection EnsureDenormalisedEntitiesHaveAConsumer(this IServiceCollection services)
        {
            var deNormClasses = GetDenormalisedClasses();
            var consumers = GetTypesWithDenormalisedEventConsumer();

            foreach (var deNormClass in deNormClasses)
            {
                var consumer = consumers.Find(x =>
                {
                    var modelName = x.GetCustomAttribute<DenormalisedEventConsumer>().ModelName;
                    return deNormClass.Name == modelName;
                }) ?? throw new ApplicationException($"Model {deNormClass.Name} dose not have a {nameof(DenormalisedEventConsumer)} that updates it's {nameof(DenormalizedFieldAttribute)} fields");
            }

            return services;
        }

        private static List<Type> GetDenormalisedClasses()
        {
            return [.. AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(IDenormalizedEntity).IsAssignableFrom(t)
                            && t.IsClass
                            && !t.IsAbstract)];
        }

        private static List<Type> GetTypesWithDenormalisedEventConsumer()
        {
            return [.. AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && t.GetCustomAttribute<DenormalisedEventConsumer>() != null)];
        }
    }
}
