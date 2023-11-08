namespace SEServer.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                    policy.AllowAnyHeader();
                    policy.WithExposedHeaders("grpc-status", "grpc-message");
                });
            });
            var app = builder.Build();
            app.MapReverseProxy();
            app.UseCors();
            app.UseStaticFiles(new StaticFileOptions()
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
            app.Run();
        }
    }
}