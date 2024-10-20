﻿using Microsoft.EntityFrameworkCore;
using PoliceCaseManagement.Core.Entities;
using PoliceCaseManagement.Core.Interfaces;

namespace PoliceCaseManagement.Infrastructure.Data.Repositories
{
    public class CaseRepository(ApplicationDbContext context) : ICaseRepository<Case, string>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task AddAsync(Case entity)
        {
            await _context.Cases.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(string id, string userId)
        {
            var caseToDelete = await _context.Cases.FirstOrDefaultAsync(c => c.Id == id);
            if (caseToDelete is null) return false;

            caseToDelete.IsDeleted = true;
            caseToDelete.DeletedAt = DateTime.UtcNow;
            caseToDelete.DeletedById = userId;

            await _context.SaveChangesAsync();
            return true;
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsAsync(string id)
        {
            var caseExists = await _context.Cases.AnyAsync(x => x.Id == id);

            return caseExists;
        }

        public async Task<Case?> GetByIdAsync(string id)
        {
            var caseToGet = await _context.Cases.FirstOrDefaultAsync(x => x.Id == id);

            return caseToGet;
        }

        public async Task<Case?> GetCaseWithDetailsByIdAsync(string id)
        {
            var caseToGetWithDetails = await _context.Cases
                .Include(x => x.CaseVehicles)
                .Include(x => x.CasePersons)
                .Include(x => x.CaseUsers)
                .Include(x => x.CaseTags)
                .Include(x => x.CaseEvidences)
                .Include(x => x.CaseCrimeScenes)
                .Include(x => x.CaseDocuments)
                .Include(x => x.Reports)
                .Include(x => x.Department)
                .Include(x => x.CreatedBy)
                .Include(x => x.LastEditedBy)
                .Include(x => x.DeletedBy)
                .FirstOrDefaultAsync(x => x.Id == id);

            return caseToGetWithDetails;
        }

        public async Task UpdateAsync(Case updatedEntity)
        {
            _context.Cases.Update(updatedEntity);
            await _context.SaveChangesAsync();
        }
    }
}
