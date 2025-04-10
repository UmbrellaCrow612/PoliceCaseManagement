namespace Cases.Core.Services
{
    /// <summary>
    /// Standard result object for a cases service
    /// </summary>
    public interface IServiceResult
    {
        public bool Succeeded { get; set; }

        public ICollection<IServiceError> Errors { get; set; }

    }
}
