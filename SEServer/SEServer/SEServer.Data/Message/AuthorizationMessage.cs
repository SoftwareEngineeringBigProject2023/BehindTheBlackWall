using MessagePack;

namespace SEServer.Data;

[MessagePackObject]
public class AuthorizationMessage : IMessage
{
    [Key(0)]
    public int UserId { get; set; }
}