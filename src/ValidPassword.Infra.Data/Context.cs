using Microsoft.EntityFrameworkCore;
using ValidPassword.Domain.Entities;

namespace ValidPassword.Infra.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
    }
}
