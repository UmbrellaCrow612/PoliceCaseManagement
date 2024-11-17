namespace Email.Service.Models
{
    public record EmailAttachment(
     string FileName,
     byte[] Content,
     string ContentType);
}
