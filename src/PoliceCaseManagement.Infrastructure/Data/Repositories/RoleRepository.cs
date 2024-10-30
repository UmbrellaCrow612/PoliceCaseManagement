using Microsoft.EntityFrameworkCore;
using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Core.Entities.Joins;
using PoliceCaseManagement.Infrastructure.Interfaces;

namespace PoliceCaseManagement.Infrastructure.Data.Repositories
{
    public class RoleRepository(ApplicationDbContext context) : BaseRepository<Role>(context), IRoleRepository
    {
        public async Task LinkUserToRoleAsync(string userId, string roleName)
        {
            var roleId = await _context.Roles.Where(x => x.Name == roleName).Select(x => x.Id).FirstOrDefaultAsync() ?? throw new ApplicationException("Role dose not exist");

            var userRoleToCreate = new UserRole
            {
                RoleId = roleId,
                UserId = userId,
            };

            await _context.UserRoles.AddAsync(userRoleToCreate);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RoleNameExistsAsync(string roleName)
        {
            return await _context.Roles.AnyAsync(x => x.Name == roleName);
        }

        public async Task<bool> UserLinkedToRoleAsync(string userId, string roleName)
        {
            var roleId = await _context.Roles.Where(x => x.Name == roleName).Select(x => x.Id).FirstOrDefaultAsync() ?? throw new ApplicationException("Role dose not exist");

            return await _context.UserRoles.AnyAsync(x => x.UserId == userId && x.RoleId == roleId);
        }
    }
}
