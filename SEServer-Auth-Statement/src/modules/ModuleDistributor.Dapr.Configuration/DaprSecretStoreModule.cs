using Dapr.Client;
using Dapr.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
namespace ModuleDistributor.Dapr.Configuration
{
    [DependsOn(typeof(DaprModule))]
    public class DaprSecretStoreModule : CustomModule
    {
        public override void ConfigureServices(ServiceContext context)
        {
            using (var scope = context.Services.BuildServiceProvider().CreateScope())
            {
                DaprOptions options = new DaprOptions();
                context.Configuration.GetSection(nameof(DaprOptions)).Bind(options);
                var daprClient = scope.ServiceProvider.GetRequiredService<DaprClient>();
                context.ConfigurationBuilder.AddDaprSecretStore(options.SecretStore!, daprClient);
            }
        }
    }
}