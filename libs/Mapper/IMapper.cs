namespace Mapper
{
    /// <summary>
    /// Our way of mapping - inherit this and pass the mapping props and implement it
    /// </summary>
    /// <typeparam name="TBase">The base model</typeparam>
    /// <typeparam name="TDto">The dto that is used to send the model</typeparam>
    /// <typeparam name="TUpdateDto">The dto used to update the model fields</typeparam>
    /// <typeparam name="TCreateDto">The dto used to create the model</typeparam>
    public interface IMapper<TBase, TDto, TUpdateDto, TCreateDto>
    {
        public TDto ToDto(TBase @base);

        public void Update(TBase @base, TUpdateDto updateDto);

        public TBase Create(TCreateDto createDto);
    }
}
