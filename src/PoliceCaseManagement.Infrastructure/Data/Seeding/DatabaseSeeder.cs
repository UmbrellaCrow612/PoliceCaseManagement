using Microsoft.EntityFrameworkCore;
using PoliceCaseManagement.Core.Constants;
using PoliceCaseManagement.Core.Entities;

namespace PoliceCaseManagement.Infrastructure.Data.Seeding
{
    /// <summary>
    /// Provides robust, idempotent seeding mechanisms for EF Core
    /// </summary>
    public static class DatabaseSeeder
    {
        /// <summary>
        /// Seed initial data with built-in safeguards
        /// </summary>
        public static void SeedData(ModelBuilder modelBuilder)
        {
            SeedRoles(modelBuilder);
        }

        /// <summary>
        /// Seed initial roles using a foreach loop
        /// </summary>
        private static void SeedRoles(ModelBuilder modelBuilder)
        {
            // Get the existing roles that would be added
            var rolesToSeed = RoleConstants.Roles
                .Select(roleName => new Role
                {
                    Name = roleName,
                })
                .ToArray();

            // Retrieve existing roles from the database to prevent duplicates
            var existingRoles = modelBuilder.Entity<Role>()
                .Metadata
                .GetSeedData()
                .OfType<Role>()
                .Select(r => r.Name)
                .ToHashSet();

            // Filter out roles that already exist
            var uniqueRoles = rolesToSeed
                .Where(r => !existingRoles.Contains(r.Name))
                .ToArray();

            // Only seed roles that are not already in the database
            if (uniqueRoles.Length != 0)
            {
                modelBuilder.Entity<Role>().HasData(uniqueRoles);
            }
        }

    }
}
