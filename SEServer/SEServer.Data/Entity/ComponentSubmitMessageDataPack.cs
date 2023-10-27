using System.Collections.Generic;

namespace SEServer.Data;

public class ComponentSubmitMessageDataPack
{
    public int TypeCode { get; set; }
    public Dictionary<CId, List<ISubmitMessage>> SubmitMessages { get; set; } = new();
    
    public void AddSubmitMessage(CId id, List<ISubmitMessage> messages)
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