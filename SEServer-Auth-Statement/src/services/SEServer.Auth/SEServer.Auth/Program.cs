using ModuleDistributor;
using ModuleDistributor.Serilog;

namespace SEServer.Auth
{
    public class Program
    {
        [Serilog]
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            await builder.ConfigureServiceAsync<SEServerAuthModule>();
            var app = builder.Build();
            await app.OnApplicationInitializationAsync();
            await app.RunAsync();
        }
    }
}