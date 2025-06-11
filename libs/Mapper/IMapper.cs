namespace Mapper
{
    /// <summary>
    /// Defines a mapping contract between a domain model and its corresponding Data Transfer Objects (DTOs)
    /// for creation, updating, and retrieval operations.
    /// Implement this interface to handle transformations between <typeparamref name="TBase"/> and its DTO representations.
    /// </summary>
    /// <typeparam name="TBase">The domain model type.</typeparam>
    /// <typeparam name="TDto">The DTO type used to represent the model when retrieving data.</typeparam>
    /// <typeparam name="TUpdateDto">The DTO type used for updating the model.</typeparam>
    /// <typeparam name="TCreateDto">The DTO type used for creating a new instance of the model.</typeparam>
    public interface IMapper<TBase, TDto, TUpdateDto, TCreateDto>
    {
        /// <summary>
        /// Converts a <typeparamref name="TBase"/> instance to its corresponding <typeparamref name="TDto"/> representation.
        /// </summary>
        /// <param name="base">The domain model instance to convert.</param>
        /// <returns>The DTO representation of the domain model.</returns>
        public TDto ToDto(TBase @base);

        /// <summary>
        /// Applies data from a <typeparamref name="TUpdateDto"/> to an existing <typeparamref name="TBase"/> instance.
        /// </summary>
        /// <param name="base">The existing domain model to update.</param>
        /// <param name="updateDto">The DTO containing the updated data.</param>
        public void Update(TBase @base, TUpdateDto updateDto);

        /// <summary>
        /// Creates a new <typeparamref name="TBase"/> instance using data from a <typeparamref name="TCreateDto"/>.
        /// </summary>
        /// <param name="createDto">The DTO containing data for the new model instance.</param>
        /// <returns>A new instance of the domain model.</returns>
        public TBase Create(TCreateDto createDto);
    }
}
