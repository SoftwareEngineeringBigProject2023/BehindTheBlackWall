using System.Collections.Generic;
using MessagePack;
using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.Data.Message;

namespace SEServer.GameData.Component;

[MessagePackObject]
public class PlayerNotifyComponent : INotifyComponent
{
    [Key(0)]
    public CId Id { get; set; }
    [Key(1)]
    public EId EntityId { get; set; }
    [Key(2)]
    public PlayerId Owner { get; set; }
    [Key(3)]
    public Queue<NotifyData> NotifyMessages { get; set; } = new();
}   