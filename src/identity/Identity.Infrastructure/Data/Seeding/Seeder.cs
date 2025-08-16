using Authorization;
using Identity.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data.Seeding
{
    public class Seeder(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
        private readonly Dictionary<string, ApplicationUser> users = new Dictionary<string, ApplicationUser>()
        {
            {
                Roles.Admin,
                new ApplicationUser
                {
                    Email = "admin@example.com",
                    PhoneNumber = "987654321",
                    UserName = "admin",
                }
            }
        };

        public async Task SeedUsersAndThereRolesAsync()
        {
            foreach (var user in users)
            {
                if (!await _userManager.Users.AnyAsync(x => x.Email == user.Value.Email))
                {
                    var createUserResult = await _userManager.CreateAsync(user.Value, "Password@123");
                    if (!createUserResult.Succeeded)
                    {
                        throw new ApplicationException("Failed to create default users");
                    }
                    var assignRoleResult = await _userManager.AddToRoleAsync(user.Value, user.Key);
                    if (!assignRoleResult.Succeeded)
                    {
                        throw new ApplicationException("Failed to link to role");
                    }
                }
            }
        }

        public async Task SeeRolesAsync()
        {
            foreach (var role in Roles.AllRoles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    var _role = new ApplicationRole
                    {
                        Name = role
                    };

                    var result = await _roleManager.CreateAsync(_role);
                    if (!result.Succeeded)
                    {
                        throw new ApplicationException("Failed to create default role");
                    }
                }
            }
        }

    }
}
