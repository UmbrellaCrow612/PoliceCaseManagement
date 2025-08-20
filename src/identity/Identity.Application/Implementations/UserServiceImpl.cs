using Identity.Application.Codes;
using Identity.Core.Models;
using Identity.Core.Services;
using Identity.Core.ValueObjects;
using Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Pagination.Abstractions;
using Results.Abstractions;

namespace Identity.Application.Implementations
{
    /// <summary>
    /// Business implementation of the contract <see cref="IUserService"/> - test this, as well when using it else where only use the <see cref="IUserService"/>
    /// interface not this class
    /// </summary>
    public class UserServiceImpl(
        IdentityApplicationDbContext dbContext,
        IUserValidationService userValidationService,
        IPasswordHasher passwordHasher
        ) : IUserService
    {
        private readonly IdentityApplicationDbContext _dbcontext = dbContext;
        private readonly IUserValidationService _userValidationService = userValidationService;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;

        public bool CheckPassword(ApplicationUser user, string password)
        {
            return _passwordHasher.Verify(user.PasswordHash, password);
        }

        public async Task<IResult> CreateAsync(ApplicationUser user, string password)
        {
            var result = new Result();

            var userValidation = _userValidationService.Validate(user);
            if (!userValidation.Succeeded)
            {
                result.Errors.AddRange(userValidation.Errors);
                return result;
            }

            var passwordValidation = _userValidationService.ValidatePassword(password);
            if (!passwordValidation.Succeeded)
            {
                result.Errors.AddRange(passwordValidation.Errors);
                return result;
            }

            if (!string.IsNullOrWhiteSpace(user.PasswordHash)) // users should not set this
            {
                result.AddError(BusinessRuleCodes.UserCreation);
                return result;
            }

            if (await IsUsernameTakenAsync(user.UserName))
            {
                result.AddError(BusinessRuleCodes.UserCreation, "Username taken");
                return result;
            }

            if (await IsEmailTakenAsync(user.Email))
            {
                result.AddError(BusinessRuleCodes.UserEmailTaken, "Email taken");
                return result;
            }

            if (await IsPhoneNumberTakenAsync(user.PhoneNumber))
            {
                result.AddError(BusinessRuleCodes.UserPhoneNumberTaken, "Phone number taken");
                return result;
            }

            var hashedPassword = _passwordHasher.Hash(password);
            user.PasswordHash = hashedPassword;
            
            await _dbcontext.Users.AddAsync(user);
            await _dbcontext.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<bool> ExistsAsync(string userId)
        {
            return await _dbcontext.Users.AnyAsync(x => x.Id == userId);
        }

        public async Task<ApplicationUser?> FindByEmailAsync(string email)
        {
            return await _dbcontext.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
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

        public async Task<IResult> UpdateAsync(ApplicationUser user)
        {
            var result = new Result();

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
