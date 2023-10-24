﻿using System.Collections.Generic;
using MessagePack;

namespace SEServer.Data;

[MessagePackObject]
public class ComponentNotifyMessageDataPack
{
    [Key(0)]
    public int TypeCode { get; set; }
    [Key(1)]
    public Dictionary<CId, List<NotifyMessage>> NotifyMessages { get; set; } = new();
    
    public void AddNotifyMessage(CId id, List<NotifyMessage> messages)
    {
        if (NotifyMessages.TryGetValue(id, out var message))
        {
            message.AddRange(messages);
        }
        else
        {
            NotifyMessages.Add(id, messages);
        }
    }
}