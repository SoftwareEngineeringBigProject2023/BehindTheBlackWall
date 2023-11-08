using Microsoft.AspNetCore.Builder;
using ModuleDistributor.Grpc;

namespace ModuleDistributor.GrpcWebSocketBridge
{
    [DependsOn(typeof(GrpcModule))]
    public class GrpcWebSocketBrigdeModule : CustomModule
    {
        public override void OnApplicationInitialization(ApplicationContext context)
        {
            context.App.UseWebSockets();
            context.App.UseGrpcWebSocketRequestRoutingEnabler();
            context.App.UseRouting();
            context.App.UseGrpcWebSocketBridge();
        }
    }
}