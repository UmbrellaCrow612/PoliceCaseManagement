namespace Events.Core
{
    /// <summary>
    /// Used to mark the class that will update the <see cref="DenormalizedFieldAttribute"/>
    /// </summary>
    /// <param name="modelName">The model it is updating</param>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class DenormalisedEventConsumer(string modelName) : Attribute
    {
        public string ModelName { get; } = modelName;
    }
}
