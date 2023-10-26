using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SEServer.Auth.EntityFrameworkCore
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder().Build();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>();
            options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
            return new ApplicationDbContext(options.Options);
        }
    }
}
