using PeopleMgt.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PeopleMgt.Repositories
{
    /// <summary>
    /// Repo to handle user related data operations. abstracts the ORM.
    /// </summary>
    public interface IUserRepository
    {
        Task<Tuple<IEnumerable<User>, PageMetadata>> GetUsersAsync(PageParameters pageParams);
        Task<User> GetUserByIdAsync(int id);
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
        Task<bool> IsExistingUser(int id);

    }
}
