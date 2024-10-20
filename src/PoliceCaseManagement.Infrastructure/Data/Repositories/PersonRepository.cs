using Microsoft.EntityFrameworkCore;
using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Core.Interfaces;

namespace PoliceCaseManagement.Infrastructure.Data.Repositories
{
    public class PersonRepository(ApplicationDbContext context) : IPersonRepository<Person, string>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task AddAsync(Person entity)
        {
            await _context.Persons.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(string id, string userId)
        {
            var personToDelete = await _context.Persons.FirstOrDefaultAsync(x => x.Id == id);
            if (personToDelete is null) return false;

            personToDelete.IsDeleted = true;
            personToDelete.DeletedAt = DateTime.UtcNow;
            personToDelete.DeletedById = userId;

            await _context.SaveChangesAsync();
            return true;
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsAsync(string id)
        {
            var personsExists = await _context.Persons.AnyAsync(x => x.Id == id);

            return personsExists;
        }

        public async Task<Person?> GetByIdAsync(string id)
        {
            var personToGet = await _context.Persons.FirstOrDefaultAsync(x => x.Id == id);

            return personToGet;
        }

        public async Task UpdateAsync(Person updatedEntity)
        {
            _context.Persons.Update(updatedEntity);
            await _context.SaveChangesAsync();
        }
    }
}
