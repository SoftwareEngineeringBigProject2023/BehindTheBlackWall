using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ModuleDistributor.Grpc;
using SEServer.Auth.Protos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Cysharp.Text;
using ModuleDistributor.Logging;
using SEServer.Statements.Domain.Shared;
using SEServer.Statements.EntityFrameworkCore;
using SEServer.Statements.Domain;
using Microsoft.AspNetCore.Authorization;

namespace SEServer.Auth
{
    [GrpcService]
    internal class AuthenticationGrpcService : AuthenticationService.AuthenticationServiceBase, ILoggerProxy<AuthenticationGrpcService>
    {
        private readonly JwtTokenOptions _options;
        private readonly ApplicationDbContext _context;

        public ILogger<AuthenticationGrpcService> Logger { get; }
        ILogger ILoggerProxy.Logger
            => Logger;

        public AuthenticationGrpcService(ApplicationDbContext context, IOptions<JwtTokenOptions> options, ILogger<AuthenticationGrpcService> logger)
        {
            _options = options.Value;
            _context = context;
            Logger = logger;
        }

        [ExLogging]
        [AllowAnonymous]
        public override async Task<SignInResponse> SignIn(SignInRequest request, ServerCallContext context)
        {
            Player? user = await _context.Set<Player>().AsNoTracking().FirstOrDefaultAsync(entity => entity.UserName == request.UserName);
            if (user is null)
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Cannot find user."));

            if (user.Password != EncryptBySHA1(request.Password))
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Password not correct."));

            var response = new SignInResponse
            {
                AccessToken = CreateAccessToken(request.UserName, user.Id!)
            };

            Logger.LogInformation($"User {request.UserName} SignIn.");
            return response;
        }

        [ExLogging]
        [AllowAnonymous]
        public override async Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            var set = _context.Set<Player>();
            Player? user = await set.AsNoTracking().FirstOrDefaultAsync(entity => entity.UserName == request.UserName);
            if (user is not null)
                throw new RpcException(new Status(StatusCode.Unauthenticated, "User name is already exist."));

            user = new Player
            {
                Id = Guid.NewGuid().ToString(),
                UserName = request.UserName,
                Password = EncryptBySHA1(request.Password)
            };

            await set.AddAsync(user);
            await _context.SaveChangesAsync();

            var response = new RegisterResponse
            {
                AccessToken = CreateAccessToken(request.UserName, user.Id)
            };

            Logger.LogInformation($"Player {request.UserName} Register.");
            return response;
        }

        private string CreateAccessToken(string userName, string id)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var claims = new Claim[]
            {
                new(ClaimTypes.Name, userName),
                new(ClaimTypes.NameIdentifier, id)
            };

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
