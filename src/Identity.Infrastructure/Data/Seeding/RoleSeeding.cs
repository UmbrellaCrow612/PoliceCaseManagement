using Identity.Core.Constants;
using Microsoft.AspNetCore.Identity;

namespace Identity.Infrastructure.Data.Seeding
{
    public class RoleSeeding(RoleManager<IdentityRole> roleManager)
    {
        public async void SeedRoles()
        {
            var roles = RolesConstant.AllRoles;

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = role });
                }
            }
        }
    }
}
