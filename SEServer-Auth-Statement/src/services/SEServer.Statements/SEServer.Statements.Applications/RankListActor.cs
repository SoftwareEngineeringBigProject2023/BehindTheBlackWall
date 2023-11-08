using Dapr.Actors.Runtime;
using Microsoft.Extensions.Logging;
using ModuleDistributor.Logging;
using SEServer.Statements.Applications.Abstracts;
using SEServer.Statements.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEServer.Statements.Applications
{
    internal class RankListActor : Actor, IRankListActor
    {
        private Dictionary<string, RankScore> _rankList = new Dictionary<string, RankScore>();

        public RankListActor(ActorHost host) : base(host)
        {
        }

        protected override async Task OnActivateAsync()
        {
            await StateManager.GetOrAddStateAsync(nameof(_rankList), _rankList);
            Logger.LogInformation($"Activating WorldActor");
        }

        protected override async Task OnDeactivateAsync()
        {
            await StateManager.SetStateAsync(nameof(_rankList), _rankList);
            await StateManager.SaveStateAsync();
            Logger.LogInformation($"Deactivating WorldActor");
        }

        public Task<RankScore> GetOrAddScoreAsync(string actorId, Player user)
        {
            if (_rankList.TryGetValue(actorId, out RankScore? score))
                return Task.FromResult(score);
            
            score = new RankScore
            {
                UserName = user.UserName,
                ImagePath = user.ImagePath,
                TotalScore = 0,
                TotalKDA = 0
            };

            _rankList.Add(actorId, score);
            return Task.FromResult(score);
        }

        public Task<RankScore> UpadateScoreAsync(string actorId, PlayerRoomPackage package)
        {
            if (_rankList.TryGetValue(actorId, out RankScore? score))
            {
                score.TotalScore += package.Score;
                score.TotalKDA += package.KDA;
                return Task.FromResult(score);  
            }

            throw new ArgumentNullException("Cannot find player rank.");
        }

        public Task<List<RankScore>> GetRankListByScoreAsync(int skip, int take, string? searchName = null)
        {
            if (searchName is not null)
                return Task.FromResult((from rank in _rankList.Values
                                        where rank.UserName is not null && rank.UserName.Contains(searchName)
                                        orderby rank.TotalScore descending
                                        select rank).Skip(skip).Take(take).ToList());
            else
                return Task.FromResult((from rank in _rankList.Values
                                        orderby rank.TotalScore descending
                                        select rank).Skip(skip).Take(take).ToList());

        }

        public Task<List<RankScore>> GetRankListByKDAAsync(int skip, int take, string? searchName = null)
        {
            if (searchName is not null)
                return Task.FromResult((from rank in _rankList.Values
                                        where rank.UserName is not null && rank.UserName.Contains(searchName)
                                        orderby rank.TotalKDA descending
                                        select rank).Skip(skip).Take(take).ToList());
            else
                return Task.FromResult((from rank in _rankList.Values
                                        orderby rank.TotalKDA descending
                                        select rank).Skip(skip).Take(take).ToList());

        }
    }
}
