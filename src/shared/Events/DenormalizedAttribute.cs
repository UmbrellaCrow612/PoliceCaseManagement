namespace Events
{
    /// <summary>
    /// Indicates that a property is a denormalized copy of data from another source model.
    /// This implies it needs to be updated when the source data changes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DenormalizedFieldAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the entity or model in the source service
        /// from which this field's data originates.
        /// </summary>
        public string SourceEntityName { get; }

        /// <summary>
        /// Gets the name of the property in the source entity
        /// from which this field's data originates.
        /// </summary>
        public string SourcePropertyName { get; }

        /// <summary>
        /// (Optional) Gets the name of the microservice that owns the source data.
        /// Useful for context in larger systems.
        /// </summary>
        public string? SourceServiceName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DenormalizedFieldAttribute"/> class.
        /// </summary>
        /// <param name="sourceEntityName">The name of the source entity (e.g., "Product", "User").</param>
        /// <param name="sourcePropertyName">The name of the property in the source entity (e.g., "Name", "Email").</param>
        /// <param name="sourceServiceName">(Optional) The name of the microservice owning the source data.</param>
        public DenormalizedFieldAttribute(string sourceEntityName, string sourcePropertyName, string? sourceServiceName = null)
        {
            if (string.IsNullOrWhiteSpace(sourceEntityName))
                throw new ArgumentNullException(nameof(sourceEntityName));
            if (string.IsNullOrWhiteSpace(sourcePropertyName))
                throw new ArgumentNullException(nameof(sourcePropertyName));

            SourceEntityName = sourceEntityName;
            SourcePropertyName = sourcePropertyName;
            SourceServiceName = sourceServiceName;
        }
    }
}
