namespace Events
{
    /// <summary>
    /// Event fired off when you want to send a email
    /// </summary>
    /// <param name="To">The email that you want to send it to</param>
    /// <param name="Subject">The subject title</param>
    /// <param name="Body">The content of the body</param>
    public record SendEmailEvent(string To, string Subject, string Body);
}
