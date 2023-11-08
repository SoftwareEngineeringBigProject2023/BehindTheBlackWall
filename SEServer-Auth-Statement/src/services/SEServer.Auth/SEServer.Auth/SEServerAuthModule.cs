using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ModuleDistributor;
using ModuleDistributor.Dapr.Configuration;
using ModuleDistributor.Grpc;
using ModuleDistributor.GrpcWebSocketBridge;
using ModuleDistributor.Serilog;
using SEServer.Statements.Domain.Shared;
using SEServer.Statements.EntityFrameworkCore;
using System.Text;

namespace SEServer.Auth
{
    [DependsOn(typeof(DaprSecretStoreModule),
        typeof(SerilogModule),
        typeof(GrpcWebSocketBrigdeModule),
        typeof(GrpcServiceModule<SEServerAuthModule>),
        typeof(SEServerStatementsEntityFrameworkCoreModule))]
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
                    options.RequireHttpsMetadata = false;
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
            context.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                    policy.AllowAnyHeader();
                    policy.WithExposedHeaders("grpc-status", "grpc-message");
                });
            });
        }

        public override void OnApplicationInitialization(ApplicationContext context)
        {
            context.App.UseAuthentication();
            context.App.UseAuthorization();
            context.App.UseCors();
            context.App.UseStaticFiles(new StaticFileOptions()
            {
                ServeUnknownFileTypes = true,
                DefaultContentType = "application/octet-stream",
                OnPrepareResponse = (ctx) =>
                {
                    if (ctx.File.Name.EndsWith(".br"))
                    {
                        ctx.Context.Response.Headers.ContentEncoding = "br";
                    }
                    if (ctx.File.Name.Contains(".wasm")) ctx.Context.Response.Headers.ContentType = "application/wasm";
                    if (ctx.File.Name.Contains(".js")) ctx.Context.Response.Headers.ContentType = "application/javascript";
                }
            });
        }
    }
}
