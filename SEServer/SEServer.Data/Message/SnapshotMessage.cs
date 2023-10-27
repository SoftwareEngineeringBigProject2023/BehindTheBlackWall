using MessagePack;
using SEServer.Data.Interface;

namespace SEServer.Data.Message;

[MessagePackObject]
public class SnapshotMessage : IWorldMessage
{
    [Key(0)]
    public Snapshot Snapshot { get; set; } = null!;
    [Key(1)]
    public PlayerId PlayerId { get; set; }
}