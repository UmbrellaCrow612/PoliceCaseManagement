namespace Events.Core
{
    /// <summary>
    /// Used to mark model or entity that it contains Denormalized data from a main entity and if said main entity is changed the local copy should
    /// be changed as well, as well use tha attribute toi mark Denormalized fields 
    /// </summary>
    public interface IDenormalizedEntity
    {
    }
}
