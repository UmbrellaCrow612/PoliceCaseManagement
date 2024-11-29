namespace Evidence.API.DTOs.Update
{
    public class UpdatePhotoDto
    {
        public required string FileName { get; set; }
        public required string FilePath { get; set; }
        public required string FileExtension { get; set; }
        public required string Description { get; set; }
        public required DateTime TakenAt { get; set; }
        public required string TakenBy { get; set; }
        public required string Location { get; set; }
    }
}
