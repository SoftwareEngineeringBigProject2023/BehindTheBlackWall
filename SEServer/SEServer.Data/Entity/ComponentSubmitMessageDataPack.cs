﻿using System.Collections.Generic;
using MessagePack;

namespace SEServer.Data;

[MessagePackObject]
public class ComponentSubmitMessageDataPack
{
    [Key(0)]
    public int TypeCode { get; set; }
    [Key(1)]
    public Dictionary<CId, List<SubmitMessage>> SubmitMessages { get; set; } = new();
    
    public void AddSubmitMessage(CId id, List<SubmitMessage> messages)
    {
        if (SubmitMessages.TryGetValue(id, out var message))
        {
            message.AddRange(messages);
        }
        else
        {
            SubmitMessages.Add(id, messages);
        }
    }
}