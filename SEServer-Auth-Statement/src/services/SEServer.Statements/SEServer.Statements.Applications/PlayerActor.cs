using Dapr.Actors;
using Dapr.Actors.Runtime;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SEServer.Statements.Applications.Abstracts;
using SEServer.Statements.Domain;
using SEServer.Statements.Domain.Shared;
using System.Runtime.CompilerServices;

namespace SEServer.Statements.Applications
{
    internal class PlayerActor : Actor, IPlayerActor
    {
        private PlayerPackage _package = new PlayerPackage();
        private PlayerRoomPackage _roomPackage = new PlayerRoomPackage();

        public PlayerActor(ActorHost host) : base(host)
        {
        }

        protected override async Task OnActivateAsync()
        {
            _package = await StateManager.GetOrAddStateAsync(nameof(_package), _package);
            _roomPackage = await StateManager.GetOrAddStateAsync(nameof(_roomPackage), _roomPackage);
            Logger.LogInformation($"Activating PlayerActor Id: {Id}");
        }

        protected override async Task OnDeactivateAsync()
        {
            await StateManager.SetStateAsync(nameof(_package), _package);
            await StateManager.SetStateAsync(nameof(_roomPackage), _roomPackage);
            await StateManager.SaveStateAsync();
            Logger.LogInformation($"Deactivating  PlayerActor Id: {Id}");
        }

        public async Task<EnterGameResponse> EnterGameAsync(Player user)
        {
            var rankListActor = ProxyFactory.CreateActorProxy<IRankListActor>(new ActorId(nameof(RankListActor)), nameof(RankListActor));
            var score = await rankListActor.GetOrAddScoreAsync(Id.GetId(), user);

            if (_package.FirstEnter)
            {
                _package.Rank = score;
                _package.FirstEnter = false;
                _package.Modules = new List<WeaponModule>();
            }

            return new EnterGameResponse
            {
                UserName = score.UserName,
                ImagePath = score.ImagePath,
                TotalScore = score.TotalScore,
                TotalKDA = score.TotalKDA
            };
        }

        public Task EnterRoomAsync(string attackModule, string defendModule, string recoverModule)
        {
            if (_roomPackage.Modules is null)
                _roomPackage.Modules = new List<WeaponModule>();

            _roomPackage.Modules.Add(new WeaponModule { ModuleName = attackModule, ModuleType = "Attack" });
            _roomPackage.Modules.Add(new WeaponModule { ModuleName = defendModule, ModuleType = "Defend" });
            _roomPackage.Modules.Add(new WeaponModule { ModuleName = recoverModule, ModuleType = "Recover" });
            return Task.CompletedTask;
        }

        public async Task<ExitRoomResponse> ExitRoomAsync()
        {
            var rankListActor = ProxyFactory.CreateActorProxy<IRankListActor>(new ActorId(nameof(RankListActor)), nameof(RankListActor));
            var score = await rankListActor.UpadateScoreAsync(Id.GetId(), _roomPackage);   
            _package.Rank = score;
            _package.Modules!.AddRange(_roomPackage.Modules!.Where(item1 => !_package.Modules.Any(item2 => item2.ModuleName == item1.ModuleName)));
            
            PlayerRoomPackage roomPackage = _roomPackage;
            _roomPackage = new PlayerRoomPackage();
            
            return new ExitRoomResponse
            {
                KDA = roomPackage.KDA,
                Score = roomPackage.Score
            };
        }

        public Task<DeathResponse> DeathAsync()
        {
            PlayerRoomPackage roomPackage = _roomPackage;
            _roomPackage = new PlayerRoomPackage();

            return Task.FromResult(new DeathResponse
            {
                KDA = roomPackage.KDA,
                Score = roomPackage.Score
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task<GlobalRankResponse> CheckGlobalRankAsync()
            => Task.FromResult(new GlobalRankResponse
            {
                TotalKDA = _package.Rank!.TotalKDA,
                TotalScore = _package.Rank!.TotalScore
            });

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task<IEnumerable<GlobalWeaponModuleResponse>> CheckGlobalWeaponModuleAsync()
            => Task.FromResult(from item in _package.Modules!
                               select new GlobalWeaponModuleResponse
                               {
                                   WeaponModuleName = item.ModuleName,
                                   WeaponModuleType = item.ModuleType
                               });

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task<RoomRankResponse> CheckRoomRankAsync()
            => Task.FromResult(new RoomRankResponse
            {
                KDA = _roomPackage.KDA,
                Score = _roomPackage.Score,
            });

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task<IEnumerable<RoomWeaponModuleResponse>> CheckRoomWeaponModuleAsync()
            => Task.FromResult(from item in _roomPackage.Modules!
                               select new RoomWeaponModuleResponse
                               {
                                   WeaponModuleName = item.ModuleName,
                                   WeaponModuleType = item.ModuleType
                               });

        public Task UpdateRoomRankAsync(long score, long kda)
        {
            _roomPackage.Score = score;
            _roomPackage.KDA = kda;
            return Task.CompletedTask;
        }

        public Task UpdateRoomWeaponModuleAsync(List<WeaponModule> modules)
        {
            _roomPackage.Modules!.AddRange(modules);
            return Task.CompletedTask;
        }
    }
}
