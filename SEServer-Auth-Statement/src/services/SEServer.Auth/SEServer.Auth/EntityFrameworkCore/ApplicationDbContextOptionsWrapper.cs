using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ModuleDistributor.Dapr;
using ModuleDistributor.EntityFrameworkCore;

namespace SEServer.Auth.EntityFrameworkCore
{
    public class ApplicationDbContextOptionsWrapper : OptionsActionWrapper
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
