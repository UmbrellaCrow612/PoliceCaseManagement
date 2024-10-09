using PoliceCaseManagement.Core.Entities.Enums;

namespace PoliceCaseManagement.Core.Entities.Joins
{
    /// <summary>
    /// Join between a <see cref="Entities.CrimeScene"/> and <see cref="Entities.Person"/>
    /// </summary>
    public class CrimeScenePerson
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required CrimeScenePersonRole Role { get; set; }
        public required string CrimeSceneId { get; set; }
        public required string PersonId { get; set; }


        public CrimeScene? CrimeScene { get; set; } = null;
        public Person? Person { get; set; } = null;
    }
}
