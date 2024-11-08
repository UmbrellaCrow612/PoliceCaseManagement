using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Identity.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Identity.Core.Constants;



namespace Identity.Infrastructure.Data
{
    public class IdentityApplicationDbContext(DbContextOptions<IdentityApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<IdentityRole>().HasData(

                new IdentityRole
                {
                    Id = "1",
                    Name = InternalRoles.User
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = InternalRoles.Manager
                },
                 new IdentityRole
                 {
                     Id = "3",
                     Name = InternalRoles.Admin
                 }
            );


        }
    }
}
