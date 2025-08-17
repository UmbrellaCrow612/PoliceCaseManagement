using Identity.Application.Codes;
using Identity.Core.Models;
using Identity.Core.Services;
using Identity.Core.ValueObjects;
using Identity.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pagination.Abstractions;

namespace Identity.Application.Implementations
{
    /// <summary>
    /// Business implementation of the contract <see cref="IUserService"/> - test this, as well when using it else where only use the <see cref="IUserService"/>
    /// interface not this class
    /// </summary>
    public class UserServiceImpl(
        IdentityApplicationDbContext dbContext, 
        UserManager<ApplicationUser> userManager
        ) : IUserService
    {
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<bool> ExistsAsync(string userId)
        {
            return await _dbcontext.Users.AnyAsync(x => x.Id == userId);
        }

        public async Task<ApplicationUser?> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<ApplicationUser?> FindByIdAsync(string userId)
        {
            return await _dbcontext.Users.FindAsync(userId);
        }

        public async Task<ApplicationUser?> FindByPhoneNumberAsync(string phoneNumber)
        {
            return await _dbcontext.Users.Where(x => x.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            return await _dbcontext.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<bool> IsPhoneNumberTakenAsync(string phoneNumber)
        {
            return await _dbcontext.Users.AnyAsync(x => x.PhoneNumber == phoneNumber);
        }

        public async Task<bool> IsUsernameTakenAsync(string username)
        {
            return await _dbcontext.Users.AnyAsync(x => x.UserName == username);
        }

        public async Task<PaginatedResult<ApplicationUser>> SearchAsync(SearchUserQuery query)
        {
            var queryBuilder = _dbcontext.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.UserName))
            {
                queryBuilder = queryBuilder.Where(x => x.UserName!.Contains(query.UserName));
            }

            if (!string.IsNullOrWhiteSpace(query.Email))
            {
                queryBuilder = queryBuilder.Where(x => x.Email!.Contains(query.Email));
            }

            if (!string.IsNullOrWhiteSpace(query.PhoneNumber))
            {
                queryBuilder = queryBuilder.Where(x => x.PhoneNumber!.Contains(query.PhoneNumber));
            }

            queryBuilder = query.OrderBy switch
            {
                SearchUserQueryOrderBy.Id => queryBuilder.OrderBy(x => x.Id),
                SearchUserQueryOrderBy.UserNameAscending => queryBuilder.OrderBy(x => x.UserName),
                SearchUserQueryOrderBy.UserNameDescending => queryBuilder.OrderByDescending(x => x.UserName),
                SearchUserQueryOrderBy.UserEmailAscending => queryBuilder.OrderBy(x => x.Email),
                SearchUserQueryOrderBy.UserEmailDescending => queryBuilder.OrderByDescending(x => x.Email),
                SearchUserQueryOrderBy.UserPhoneNumberAscending => queryBuilder.OrderBy(x => x.PhoneNumber),
                SearchUserQueryOrderBy.UserPhoneNumberDescending => queryBuilder.OrderByDescending(x => x.PhoneNumber),
                _ => queryBuilder.OrderBy(x => x.Id),
            };

            int pageNumber = query.PageNumber < 1 ? 1 : query.PageNumber;
            int pageSize = query.PageSize > 100 ? 100 : query.PageSize < 1 ? 10 : query.PageSize;

            int totalRecordCount = await queryBuilder.CountAsync();
            int totalPages = (int)Math.Ceiling(totalRecordCount / (double)pageSize);

            var users = await queryBuilder
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<ApplicationUser>
            {
                HasNextPage = pageNumber < totalPages,
                HasPreviousPage = pageNumber > 1,
                Data = users,
                Pagination = new PaginationMetadata
                {
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalRecords = totalRecordCount,
                    TotalPages = totalPages
                }
            };
        }

        public async Task<UserServiceResult> UpdateAsync(ApplicationUser user)
        {
            var result = new UserServiceResult();

            var usernameTaken = await _dbcontext.Users.AnyAsync(x => x.UserName == user.UserName && x.Id != user.Id);
            if (usernameTaken)
            {
                result.AddError(BusinessRuleCodes.UserNameTaken, "Username taken");
                return result;
            }

            var emailTaken = await _dbcontext.Users.AnyAsync(x => x.Email == user.Email && x.Id != user.Id);
            if (emailTaken)
            {
                result.AddError(BusinessRuleCodes.UserEmailTaken, "Email taken");
                return result;
            }

            var phoneNumberTaken = await _dbcontext.Users.AnyAsync(x => x.PhoneNumber == user.PhoneNumber && x.Id != user.Id);
            if (phoneNumberTaken)
            {
                result.AddError(BusinessRuleCodes.UserPhoneNumberTaken, "Phone number taken");
                return result;
            }

            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }
    }
}
