using Dapr.Actors;
using SEServer.Statements.Domain;
using SEServer.Statements.Domain.Shared;

namespace SEServer.Statements.Applications.Abstracts
{
    public interface IPlayerActor : IActor
    {
        Task<EnterGameResponse> EnterGameAsync(Player user);
        Task EnterRoomAsync(string attackModule, string defendModule, string recoverModule);
        Task<ExitRoomResponse> ExitRoomAsync();
        Task<DeathResponse> DeathAsync();
        Task<GlobalRankResponse> CheckGlobalRankAsync();
        Task<IEnumerable<GlobalWeaponModuleResponse>> CheckGlobalWeaponModuleAsync();
        Task<RoomRankResponse> CheckRoomRankAsync();
        Task<IEnumerable<RoomWeaponModuleResponse>> CheckRoomWeaponModuleAsync();
        Task UpdateRoomRankAsync(long score, long kda);
        Task UpdateRoomWeaponModuleAsync(List<WeaponModule> modules);
    }
}
