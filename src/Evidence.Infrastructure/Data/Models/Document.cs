namespace Evidence.Infrastructure.Data.Models
{
    public class Document
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required  string FileName { get; set; }

        public required string FilePath { get; set; }

        public required long FileSize { get; set; }

        public required string FileExtension { get; set; }

        public required DateTime UploadDateTime { get; set; }

        public required string Type { get; set; }

        public required string Description { get; set; }
    }
}
