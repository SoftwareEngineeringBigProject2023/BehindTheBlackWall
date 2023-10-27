using System.Collections.Generic;
using MessagePack;

namespace SEServer.Data;

[MessagePackObject]
public class SyncEntityMessage : IWorldMessage
{
    [Key(0)]
    public List<EId> EntitiesToDelete { get; set; } = new();
    [Key(1)]
    public List<EId> EntitiesToCreate { get; set; } = new();
    [Key(2)]
    public List<ComponentArrayDataPack> ComponentArrayDataPacks { get; set; } = new();
    [Key(3)]
    public List<ComponentNotifyMessageDataPack> ComponentNotifyDataPacks { get; set; } = new();
}