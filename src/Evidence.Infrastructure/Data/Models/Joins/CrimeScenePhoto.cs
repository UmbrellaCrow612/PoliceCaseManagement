namespace Evidence.Infrastructure.Data.Models.Joins
{
    public class CrimeScenePhoto
    {
        public required string CrimeSceneId { get; set; }
        public required string PhotoId { get; set; }

        public CrimeScene? CrimeScene { get; set; } = null;
        public Photo? Photo { get; set; } = null;
    }
}
