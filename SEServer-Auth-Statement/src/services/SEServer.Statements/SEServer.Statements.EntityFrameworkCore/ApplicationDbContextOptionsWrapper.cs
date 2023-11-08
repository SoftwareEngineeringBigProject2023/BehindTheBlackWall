using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModuleDistributor.EntityFrameworkCore;

namespace SEServer.Statements.EntityFrameworkCore
{
    internal class ApplicationDbContextOptionsWrapper : OptionsActionWrapper
    {
        public override Action<IServiceProvider, DbContextOptionsBuilder>? OptionsAction { get; } = AddDbContextOptions;

        private static void AddDbContextOptions(IServiceProvider provider, DbContextOptionsBuilder builder)
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("SqlServer");
            builder.UseSqlServer(connectionString);
        }
    }
}
