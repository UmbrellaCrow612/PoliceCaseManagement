namespace Cases.Core.Services
{
    /// <summary>
    /// The standard service error for cases services
    /// </summary>
    public interface IServiceError
    {
        public string Code { get; set; }
        public string? Message { get; set; }
    }
}
