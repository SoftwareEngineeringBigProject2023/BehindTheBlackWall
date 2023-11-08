using Microsoft.EntityFrameworkCore;
using SEServer.Statements.Domain;

namespace SEServer.Statements.EntityFrameworkCore
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.AddUser();
    }
}
