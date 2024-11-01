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
        /// Seed initial roles with duplicate prevention and GUID ID generation
        /// </summary>
        private static void SeedRoles(ModelBuilder modelBuilder)
        {
            // Get the existing roles that would be added
            var rolesToSeed = RoleConstants.Roles
                .Select(roleName => new Role
                {
                    Name = roleName
                })
                .ToArray();

            // Retrieve existing roles from the database to prevent duplicates
            var existingRoleNames = modelBuilder.Entity<Role>()
                .Metadata
                .GetSeedData()
                .OfType<Role>()
                .Select(r => r.Name)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            // Filter out roles that already exist (case-insensitive)
            var uniqueRoles = rolesToSeed
                .Where(r => !existingRoleNames.Contains(r.Name, StringComparer.OrdinalIgnoreCase))
                .ToArray();

            // Only seed roles that are not already in the database
            if (uniqueRoles.Length != 0)
            {
                modelBuilder.Entity<Role>().HasData(uniqueRoles);
            }
        }

    }
}
