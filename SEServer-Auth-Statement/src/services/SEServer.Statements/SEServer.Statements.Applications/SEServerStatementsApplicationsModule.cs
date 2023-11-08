using ModuleDistributor;
using ModuleDistributor.Dapr.Actors;
using ModuleDistributor.DependencyInjection;
using SEServer.Statements.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEServer.Statements.Applications
{
    [DependsOn(typeof(SEServerStatementsEntityFrameworkCoreModule),
        typeof(InjectServiceModule<SEServerStatementsApplicationsModule>),
        typeof(DaprActorsModule<SEServerStatementsApplicationsModule>))]
    public class SEServerStatementsApplicationsModule : CustomModule
    {
    }
}
