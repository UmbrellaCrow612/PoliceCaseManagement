namespace Identity.API.Mappings
{
    /// <summary>
    /// Our way of mapping - inherit this and pass the mapping props and implament it
    /// </summary>
    /// <typeparam name="TBase">The base model</typeparam>
    /// <typeparam name="TDto">The class that is used to send the model</typeparam>
    /// <typeparam name="TUpdateDto">The dto used to update the model fields</typeparam>
    public interface IMapper<TBase, TDto, TUpdateDto>
    {
        public TDto ToDto(TBase @base);

        public void Update(TBase @base, TUpdateDto updateDto);
    }
}
