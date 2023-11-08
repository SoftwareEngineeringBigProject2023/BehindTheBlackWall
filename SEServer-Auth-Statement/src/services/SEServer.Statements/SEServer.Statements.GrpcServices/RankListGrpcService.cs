using Dapr.Actors;
using Dapr.Actors.Client;
using Google.Api;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using ModuleDistributor.Grpc;
using ModuleDistributor.Logging;
using SEServer.Statements.Applications.Abstracts;
using SEServer.Statements.Domain.Shared;
using SEServer.Statements.EntityFrameworkCore;

namespace SEServer.Statements.GrpcServices
{
    [GrpcService]
    internal class RankListGrpcService : RankListService.RankListServiceBase, ILoggerProxy<RankListGrpcService>
    {
        private readonly IActorProxyFactory _factory;

        public ILogger<RankListGrpcService> Logger { get; }

        ILogger ILoggerProxy.Logger
            => Logger;

        public RankListGrpcService(IActorProxyFactory factory, ILogger<RankListGrpcService> logger)
        {
            Logger = logger;
            _factory = factory;
        }

        [Authorize]
        [ExLogging]
        public override async Task GetRankListByKDA(GetRankListByKDARequest request, IServerStreamWriter<GetRankListByKDAResponse> responseStream, ServerCallContext context)
        {
            var rankListActor = _factory.CreateActorProxy<IRankListActor>(new ActorId("RankListActor"), "RankListActor");
            var result = await rankListActor.GetRankListByKDAAsync(request.Skip, request.Take, request.SearchName);
            foreach (var item in result)
            {
                var response = new GetRankListByKDAResponse
                {
                    UserName = item.UserName,
                    ImagePath = item.ImagePath,
                    TotalKDA = item.TotalKDA,
                    TotalScore = item.TotalScore
                };
                await responseStream.WriteAsync(response);
            }
        }

        [Authorize]
        [ExLogging]
        public override async Task GetRankListByScore(GetRankListByScoreRequest request, IServerStreamWriter<GetRankListByScoreResponse> responseStream, ServerCallContext context)
        {
            var rankListActor = _factory.CreateActorProxy<IRankListActor>(new ActorId("RankListActor"), "RankListActor");
            var result = await rankListActor.GetRankListByScoreAsync(request.Skip, request.Take, request.SearchName);
            foreach (var item in result)
            {
                var response = new GetRankListByScoreResponse
                {
                    UserName = item.UserName,
                    ImagePath = item.ImagePath,
                    TotalKDA = item.TotalKDA,
                    TotalScore = item.TotalScore
                };
                await responseStream.WriteAsync(response);
            }
        }
    }
}
