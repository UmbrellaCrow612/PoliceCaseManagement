using Identity.Core.Constants;
using Identity.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Infrastructure.Data.Seeding
{
    public class RoleSeeding(RoleManager<ApplicationRole> roleManager)
    {
        public async void SeedRoles()
        {
            var roles = RolesConstant.AllRoles;

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = role, ConcurrencyStamp = Guid.NewGuid().ToString() });
                }
            }
        }
    }
}
