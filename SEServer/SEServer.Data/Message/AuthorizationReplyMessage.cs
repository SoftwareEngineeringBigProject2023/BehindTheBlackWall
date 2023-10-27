using MessagePack;

namespace SEServer.Data;

[MessagePackObject]
public class AuthorizationReplyMessage : IMessage
{
    [Key(0)]
    public int UserId { get; set; }
}