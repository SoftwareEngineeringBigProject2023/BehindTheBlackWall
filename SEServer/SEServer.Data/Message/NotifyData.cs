using System;
using MessagePack;

namespace SEServer.Data.Message;

[MessagePackObject]
public class NotifyData
{
    [Key(0)]
    public int Type { get; set; }
    [Key(1)]
    public int Arg0 { get; set; }
    [Key(2)]
    public int Arg1 { get; set; }
    [Key(3)]
    public int Arg2 { get; set; }
    [Key(4)]
    public int Arg3 { get; set; }
    [Key(5)]
    public string Info { get; set; } = string.Empty;
    [Key(6)]
    public byte[] Data { get; set; } = Array.Empty<byte>();
}