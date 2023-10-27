using MessagePack;
using SEServer.Data.Interface;

namespace SEServer.Data.Message;

[MessagePackObject]
public class AuthorizationMessage : IMessage
{
    [Key(0)]
    public int UserId { get; set; }
}