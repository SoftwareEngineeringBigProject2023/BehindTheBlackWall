using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ModuleDistributor;
using ModuleDistributor.Dapr;
using ModuleDistributor.Dapr.Configuration;
using ModuleDistributor.EntityFrameworkCore;
using ModuleDistributor.Grpc;
using ModuleDistributor.GrpcWebSocketBridge;
using ModuleDistributor.HealthCheck.Dapr;
using ModuleDistributor.Serilog;
using SEServer.Auth.EntityFrameworkCore;
using SEServer.Auth.Options;
using System.Text;

namespace SEServer.Auth
{
    [DependsOn(typeof(DaprHealthCheckModule),
        typeof(DaprSecretStoreModule),
        typeof(SerilogModule),
        typeof(GrpcWebSocketBrigdeModule),
        typeof(GrpcServiceModule<SEServerAuthModule>),
        typeof(EntityFrameworkCoreModule<ApplicationDbContext, ApplicationDbContextOptionsWrapper>))]
    public class SEServerAuthModule : CustomModule
    {
        public override void ConfigureServices(ServiceContext context)
        {
            var section = context.Configuration.GetSection(nameof(JwtTokenOptions));
            JwtTokenOptions tokenOptions = new JwtTokenOptions();
            section.Bind(tokenOptions);
            context.Services.Configure<JwtTokenOptions>(section);
            context.Services
                .AddAuthorization()
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = tokenOptions.Issuer,
                        ValidAudience = tokenOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.Key!))
                    };
                });
        }

        public override async ValueTask OnApplicationInitializationAsync(ApplicationContext context)
        {
            context.App.UseAuthentication();
            context.App.UseAuthorization();
            using (var scope = context.App.ApplicationServices.CreateScope())
                await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();
        }
    }
}
