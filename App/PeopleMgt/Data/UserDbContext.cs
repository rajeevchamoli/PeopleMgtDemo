using Microsoft.EntityFrameworkCore;
using PeopleMgt.Models;

namespace PeopleMgt.Data
{
    public class UserDbContext: DbContext
    {
        public UserDbContext() : base() { }

        public UserDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> UserTable { get; set; }
        
    }
}
