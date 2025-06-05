using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Events.Core
{
    public static class Extensions 
    {
        /// <summary>
        /// Compile time check to make sure <see cref="IDenormalizedEntity"/> actually update there <see cref="DenormalizedFieldAttribute"/> use only for development check 
        /// Checks there is a consumer for it, checks that, that consumer dose modify those fields
        /// </summary>
        public static IServiceCollection EnsureDenormalisedFieldsAreUpdated(this IServiceCollection services)
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

                var fields = deNormClass.GetProperties(BindingFlags.Public | BindingFlags.Instance) // fields that need to be updated
                 .Where(p => p.GetCustomAttribute<DenormalizedFieldAttribute>() != null)
                 .Select(p => p.Name)
                 .ToList();

                foreach (var field in fields)
                {
                }

                var r = 0;

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
