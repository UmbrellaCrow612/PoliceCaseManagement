using Microsoft.EntityFrameworkCore;
using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Infrastructure.Interfaces;

namespace PoliceCaseManagement.Infrastructure.Data.Repositories
{
    public class UserRepository(ApplicationDbContext context) : BaseRepository<User>(context), IUserRepository
    {
        public async Task DeleteAsync(string userIdToDelete, string userIdOfDeleter)
        {
            var userToDelete = await _context.Users.FindAsync(userIdToDelete) ?? throw new ApplicationException("User dose not exist");

            userToDelete.IsDeleted = true;
            userToDelete.DeletedAt = DateTime.UtcNow;
            userToDelete.DeletedById = userIdOfDeleter;

            await _context.SaveChangesAsync();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<User?> GetByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<IEnumerable<string>> GetRoles(string id)
        {
            return await _context.UserRoles.Where(x => x.UserId == id).Select(x => x.Role.Name).ToListAsync();
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(x => x.Username == username);
        }
    }
}
