using ModuleDistributor;
using ModuleDistributor.Grpc;
using ModuleDistributor.GrpcWebSocketBridge;
using SEServer.Statements.Applications;

namespace SEServer.Statements.GrpcServices
{
    [DependsOn(typeof(SEServerStatementsApplicationsModule),
        typeof(GrpcServiceModule<SEServerStatementsGrpcServicesModule>),
        typeof(GrpcWebSocketBrigdeModule))]
    public class SEServerStatementsGrpcServicesModule : CustomModule
    {
    }
}
