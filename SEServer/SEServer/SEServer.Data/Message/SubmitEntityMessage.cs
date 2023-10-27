using System.Collections.Generic;
using MessagePack;

namespace SEServer.Data;

[MessagePackObject]
public class SubmitEntityMessage : IWorldMessage
{
    [Key(0)]
    public List<ComponentArrayDataPack> ComponentArrayDataPacks { get; set; } = new();
    [Key(1)]
    public List<ComponentSubmitMessageDataPack> ComponentSubmitDataPacks { get; set; } = new();
}