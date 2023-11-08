using Dapr.Actors.Client;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ModuleDistributor.Grpc;
using ModuleDistributor.Logging;
using SEServer.Statements.Applications.Abstracts;
using SEServer.Statements.Domain;
using SEServer.Statements.Domain.Shared;
using SEServer.Statements.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Dapr.Actors;
using SEServer.Statements.Applications;

namespace SEServer.Statements.GrpcServices
{
    [GrpcService]
    internal class PlayerGrpcService : PlayerService.PlayerServiceBase, ILoggerProxy<PlayerGrpcService>
    {
        private readonly IActorProxyFactory _factory;
        private readonly JwtSecurityTokenHandler _handler = new JwtSecurityTokenHandler();
        private readonly ApplicationDbContext _contex;

        public ILogger<PlayerGrpcService> Logger { get; }

        ILogger ILoggerProxy.Logger
            => Logger;

        public PlayerGrpcService(IActorProxyFactory factory, ApplicationDbContext context, ILogger<PlayerGrpcService> logger)
        {
            Logger = logger;
            _factory = factory;
            _contex = context;
        }

        [Authorize]
        [ExLogging]
        public override async Task<EnterGameResponse> EnterGame(Empty request, ServerCallContext context)
        {
            var id = _handler.GetNameIdentifier(context.RequestHeaders);
            
            Player? user = await _contex.Set<Player>().FirstOrDefaultAsync(item => item.Id == id.Value);
            if (user is null)
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Cannot find user info."));

            var playerActor = _factory.CreateActorProxy<IPlayerActor>(new ActorId(id.Value), "PlayerActor");
            return await playerActor.EnterGameAsync(user);
        }

        [Authorize]
        [ExLogging]
        public override async Task<Empty> EnterRoom(EnterRoomRequest request, ServerCallContext context)
        {
            var id = _handler.GetNameIdentifier(context.RequestHeaders);
            var playerActor = _factory.CreateActorProxy<IPlayerActor>(new ActorId(id.Value), "PlayerActor");
            await playerActor.EnterRoomAsync(request.AttackModule, request.DefendModule, request.RecoverModule);
            return new Empty();
        }

        [Authorize]
        [ExLogging]
        public override async Task<ExitRoomResponse> ExitRoom(Empty request, ServerCallContext context)
        {
            var id = _handler.GetNameIdentifier(context.RequestHeaders);
            var playerActor = _factory.CreateActorProxy<IPlayerActor>(new ActorId(id.Value), "PlayerActor");
            return await playerActor.ExitRoomAsync();
        }

        [Authorize]
        [ExLogging]
        public override async Task<DeathResponse> Death(Empty request, ServerCallContext context)
        {
            var id = _handler.GetNameIdentifier(context.RequestHeaders);
            var playerActor = _factory.CreateActorProxy<IPlayerActor>(new ActorId(id.Value), "PlayerActor");
            return await playerActor.DeathAsync();
        }

        [Authorize]
        [ExLogging]
        public override async Task<GlobalRankResponse> CheckGlobalRank(Empty request, ServerCallContext context)
        {
            var id = _handler.GetNameIdentifier(context.RequestHeaders);
            var playerActor = _factory.CreateActorProxy<IPlayerActor>(new ActorId(id.Value), "PlayerActor");
            return await playerActor.CheckGlobalRankAsync();
        }

        [Authorize]
        [ExLogging]
        public override async Task CheckGlobalWeaponModule(Empty request, IServerStreamWriter<GlobalWeaponModuleResponse> responseStream, ServerCallContext context)
        {
            var id = _handler.GetNameIdentifier(context.RequestHeaders);
            var playerActor = _factory.CreateActorProxy<IPlayerActor>(new ActorId(id.Value), "PlayerActor");
            foreach (var item in await playerActor.CheckGlobalWeaponModuleAsync())
                await responseStream.WriteAsync(item);
        }

        [Authorize]
        [ExLogging]
        public override async Task<RoomRankResponse> CheckRoomRank(Empty request, ServerCallContext context)
        {
            var id = _handler.GetNameIdentifier(context.RequestHeaders);
            var playerActor = _factory.CreateActorProxy<IPlayerActor>(new ActorId(id.Value), "PlayerActor");
            return await playerActor.CheckRoomRankAsync();
        }

        [Authorize]
        [ExLogging]
        public override async Task CheckRoomWeaponModule(Empty request, IServerStreamWriter<RoomWeaponModuleResponse> responseStream, ServerCallContext context)
        {
            var id = _handler.GetNameIdentifier(context.RequestHeaders);
            var playerActor = _factory.CreateActorProxy<IPlayerActor>(new ActorId(id.Value), "PlayerActor");
            foreach (var item in await playerActor.CheckRoomWeaponModuleAsync())
                await responseStream.WriteAsync(item);
        }

        [Authorize]
        [ExLogging]
        public override async Task<Empty> UpdateRoomRank(UpdateRoomRankRequest request, ServerCallContext context)
        {
            var id = _handler.GetNameIdentifier(context.RequestHeaders);
            var playerActor = _factory.CreateActorProxy<IPlayerActor>(new ActorId(id.Value), "PlayerActor");
            await playerActor.UpdateRoomRankAsync(request.Score, request.KDA);
            return new Empty();
        }

        [Authorize]
        [ExLogging]
        public override async Task<Empty> UpdateRoomWeaponModule(IAsyncStreamReader<UpdateRoomWeaponModuleRequest> requestStream, ServerCallContext context)
        {
            var id = _handler.GetNameIdentifier(context.RequestHeaders);
            var playerActor = _factory.CreateActorProxy<IPlayerActor>(new ActorId(id.Value), "PlayerActor");
            
            var modules = new List<WeaponModule>();
            await foreach (var item in requestStream.ReadAllAsync())
                modules.Add(new WeaponModule
                {
                    ModuleName = item.WeaponModuleName,
                    ModuleType = item.WeaponModuleType
                });

            await playerActor.UpdateRoomWeaponModuleAsync(modules);
            return new Empty();
        }
    }
}
