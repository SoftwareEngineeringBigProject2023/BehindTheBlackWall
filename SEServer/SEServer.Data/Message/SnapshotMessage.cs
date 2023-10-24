using MessagePack;

namespace SEServer.Data;

[MessagePackObject]
public class SnapshotMessage : IWorldMessage
{
    [Key(0)]
    public Snapshot? Snapshot { get; set; }
}