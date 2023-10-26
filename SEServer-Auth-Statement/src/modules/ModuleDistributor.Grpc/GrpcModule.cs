using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ModuleDistributor.Grpc
{
    public class GrpcModule : CustomModule
    {
        public override void ConfigureServices(ServiceContext context)
            => context.Services.AddGrpc();
    }
}