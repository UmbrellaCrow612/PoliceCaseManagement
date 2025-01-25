namespace Email.Service.Models
{
    public record EmailMessage(
    string To,
    string Subject,
    string Body
        );
}
