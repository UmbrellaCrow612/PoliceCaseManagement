namespace PoliceCaseManagement.Application.Exceptions
{
    /// <summary>
    /// PoliceCaseManagement error thrown when a business rule is violated.
    /// </summary>
    /// <param name="message">The details of the violation.</param>
    public class BusinessRuleException(string message) : Exception(message)
    {
    }
}
