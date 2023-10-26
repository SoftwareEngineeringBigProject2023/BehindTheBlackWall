using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ModuleDistributor.Grpc;
using SEServer.Auth.EntityFrameworkCore;
using SEServer.Auth.Options;
using SEServer.Auth.Protos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Cysharp.Text;
using ModuleDistributor.Logging;

namespace SEServer.Auth.Services
{
    [GrpcService]
    public class AuthenticationService : Authentication.AuthenticationBase, ILoggerProxy<AuthenticationService>
    {
        private readonly JwtTokenOptions _options;
        private readonly ApplicationDbContext _context;
        
        public ILogger<AuthenticationService> Logger { get; }
        ILogger ILoggerProxy.Logger 
            => Logger;

        public AuthenticationService(ApplicationDbContext context, IOptions<JwtTokenOptions> options, ILogger<AuthenticationService> logger)
        {
            _options = options.Value;
            _context = context;
            Logger = logger;
        }

        [ExLogging]
        public override async Task<SignInResponse> SignInAsync(SignInRequest request, ServerCallContext context)
        {
            Player? player = await _context.Set<Player>().AsNoTracking().FirstOrDefaultAsync(entity => entity.UserName == request.UserName);
            if (player is null)
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Cannot find player."));

            if (player.Password != EncryptBySHA1(request.Password))
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Password not correct."));
            
            var response = new SignInResponse
            {
                AccessToken = CreateAccessToken(request.UserName, player.Id),
                Id = player.Id
            };

            Logger.LogInformation($"Player {request.UserName} SignIn.");
            return response;
        }

        [ExLogging]
        public override async Task<RegisterResponse> RegisterAsync(RegisterRequest request, ServerCallContext context)
        {
            var set = _context.Set<Player>();
            Player? player = await set.AsNoTracking().FirstOrDefaultAsync(entity => entity.UserName == request.UserName);
            if (player is not null)
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Player name is already exist."));

            player = new Player
            {
                Id = Guid.NewGuid().ToString(),
                UserName = request.UserName,
                Password = EncryptBySHA1(request.Password)
            };

            await set.AddAsync(player);
            await _context.SaveChangesAsync();

            var response = new RegisterResponse
            {
                Id = player.Id,
                AccessToken = CreateAccessToken(request.UserName, player.Id)
            };

            Logger.LogInformation($"Player {request.UserName} Register.");
            return response;
        }

        private string CreateAccessToken(string userName, string? id)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var claims = new Claim[] { new(ClaimTypes.Name, userName) };

            JwtSecurityToken token = new(_options.Issuer,
                _options.Audience, claims,
                signingCredentials: new(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key!)),
                    SecurityAlgorithms.HmacSha256Signature));

            return handler.WriteToken(token);
        }

        private string EncryptBySHA1(string password)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = ZString.CreateUtf8StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                    builder.AppendFormat("{0:x2}", hash[i]);
                return builder.ToString();
            }
        }
    }
}
