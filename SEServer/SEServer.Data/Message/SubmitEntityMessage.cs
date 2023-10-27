using System.Collections.Generic;
using MessagePack;
using SEServer.Data.Interface;

namespace SEServer.Data.Message;

[MessagePackObject]
public class SubmitEntityMessage : IWorldMessage
{
    [Key(0)]
    public List<ComponentArrayDataPack> ComponentArrayDataPacks { get; set; } = new();
    [Key(1)]
    public List<ComponentSubmitMessageDataPack> ComponentSubmitDataPacks { get; set; } = new();
}