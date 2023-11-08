using Microsoft.Extensions.DependencyInjection;
using ModuleDistributor;
using ModuleDistributor.EntityFrameworkCore;

namespace SEServer.Statements.EntityFrameworkCore
{
    [DependsOn(typeof(EntityFrameworkCoreModule<ApplicationDbContext, ApplicationDbContextOptionsWrapper>))]
    public class SEServerStatementsEntityFrameworkCoreModule : CustomModule
    {
        public override async ValueTask OnApplicationInitializationAsync(ApplicationContext context)
        {
            using (var scope = context.App.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (await dbContext.Database.EnsureCreatedAsync().ConfigureAwait(false))
                    AddSeedData(dbContext);
            }
        }

        private void AddSeedData(ApplicationDbContext context) 
        {

        }
    }
}