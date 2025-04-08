namespace Email.Models
{
    public record EmailMessage(
    string To,
    string Subject,
    string Body
        );
}
