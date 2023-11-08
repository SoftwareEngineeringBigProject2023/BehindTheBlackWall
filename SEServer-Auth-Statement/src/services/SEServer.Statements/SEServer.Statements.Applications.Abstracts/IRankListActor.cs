using Dapr.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEServer.Statements.Domain;

namespace SEServer.Statements.Applications.Abstracts
{
    public interface IRankListActor : IActor
    {
        Task<RankScore> GetOrAddScoreAsync(string actorId, Player user);
        Task<RankScore> UpadateScoreAsync(string actorId, PlayerRoomPackage package);
        Task<List<RankScore>> GetRankListByKDAAsync(int skip, int take, string? searchName);
        Task<List<RankScore>> GetRankListByScoreAsync(int skip, int take, string? searchName);
    }
}
