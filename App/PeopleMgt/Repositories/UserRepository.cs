using Microsoft.EntityFrameworkCore;
using PeopleMgt.Data;
using PeopleMgt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeopleMgt.Repositories
{
    /// <summary>
    /// User Repository: currently I have not implemented tight logic of validating duplicate user by name/lastname or email.
    /// </summary>
    public class UserRepository: IUserRepository
    {
        public UserDbContext _context { get; set; }

        public UserRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsExistingUser(int id)
        {
            return await _context.UserTable.AnyAsync(e => e.Id == id);
        }

        /// <summary>
        /// Get all the user as per the request param.
        /// </summary>
        /// <param name="pagingParams"></param>
        /// <returns></returns>
        public async Task<Tuple<IEnumerable<User>, PageMetadata>> GetUsersAsync(PageParameters pagingParams)
        {
            // disabling change tracking as it hurts performance
            var source = await _context.UserTable.AsNoTracking().ToListAsync();

            // Model validators are doing most of the validation.
            if (!string.IsNullOrEmpty(pagingParams.Filter))
            {

                source = source.Where(usrInfo => (!string.IsNullOrEmpty(usrInfo.Email)) && usrInfo.Email.Contains(pagingParams.Filter, StringComparison.OrdinalIgnoreCase)
                                          || (!string.IsNullOrEmpty(usrInfo.FirstName)) && usrInfo.FirstName.Contains(pagingParams.Filter, StringComparison.OrdinalIgnoreCase)
                                          || (!string.IsNullOrEmpty(usrInfo.LastName)) && usrInfo.LastName.Contains(pagingParams.Filter, StringComparison.OrdinalIgnoreCase)
                                          || (!string.IsNullOrEmpty(usrInfo.Interests)) && usrInfo.Interests.Contains(pagingParams.Filter, StringComparison.OrdinalIgnoreCase)
                                          || (!string.IsNullOrEmpty(usrInfo.Address)) && usrInfo.Address.Contains(pagingParams.Filter, StringComparison.OrdinalIgnoreCase)
                                          ).ToList();
            }

            if (!string.IsNullOrEmpty(pagingParams.SortColumn))
            {
                // can do better later using : https://stackoverflow.com/questions/264745/bindingflags-ignorecase-not-working-for-type-getproperty

                pagingParams.SortColumn = Utility.NormalizeSortPropertyNameForUserEntity(pagingParams.SortColumn);

                source = pagingParams.SortOrder == "desc"
                    ? source.OrderByDescending(usr => usr.GetType().GetProperty(pagingParams.SortColumn).GetValue(usr, null)).ToList()
                    : source.OrderBy(p => p.GetType().GetProperty(pagingParams.SortColumn).GetValue(p, null)).ToList();
            }

            return CreatePage(pagingParams, source);
        }

        /// <summary>
        /// create required page from the fetched data according to client request .
        /// </summary>
        /// <param name="pagingParams"></param>
        /// <param name="source"></param>
        /// <param name="items"></param>
        /// <param name="paginationMetadata"></param>
        private static Tuple<IEnumerable<User>, PageMetadata> CreatePage(PageParameters pagingParams, List<User> source)
        {

            IEnumerable<User> items;
            PageMetadata paginationMetadata;

            int count = source.Count();
            int CurrentPage = pagingParams.PageNumber;
            int PageSize = pagingParams.PageSize;
            int TotalCount = count;
            int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            items = source.Skip((CurrentPage - 1) * PageSize).Take(PageSize);
            var previousPage = CurrentPage > 1 ? true : false;
            var nextPage = CurrentPage < TotalPages ? true : false;

            paginationMetadata = new PageMetadata
            {
                Current = CurrentPage,
                Previous = previousPage,
                Next = nextPage,
                Size = PageSize,
                PageCount = TotalPages,
                RecordCount = TotalCount
            };

            return new Tuple<IEnumerable<User>, PageMetadata>(items, paginationMetadata);
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _context.UserTable.AsNoTracking().
                SingleOrDefaultAsync(p => p.Id == id);
            return user ?? new User();
        }

        /// <summary>
        /// Creates new user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task CreateUserAsync(User user)
        {
            _context.UserTable.Add(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates the existing user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task UpdateUserAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
        
        /// <summary>
        /// Delete the user record from data source.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task DeleteUserAsync(User user)
        {
            _context.UserTable.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
