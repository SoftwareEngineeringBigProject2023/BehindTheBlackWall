using System.Collections.Generic;

namespace SEServer.Data;

public static class ComponentExtension
{
    public static List<NotifyData> TakeAllNotifyMessages(this INotifyComponent component)
    {
        var messages = new List<NotifyData>();
        messages.AddRange(component.NotifyMessages);
        component.NotifyMessages.Clear();

        return messages;
    }
    
    public static void ReceiveNotifyMessages(this INotifyComponent component, List<NotifyData> messages)
    {
        foreach (var notifyMessage in messages)
        {
            component.NotifyMessages.Enqueue(notifyMessage);
        }
    }
    
    public static List<SubmitData> TakeAllSubmitMessages(this ISubmitComponent component)
    {
        var messages = new List<SubmitData>();
        messages.AddRange(component.SubmitMessages);
        component.SubmitMessages.Clear();

        return messages;
    }
    
    public static void ReceiveSubmitMessages(this ISubmitComponent component, List<SubmitData> messages)
    {
        foreach (var submitMessage in messages)
        {
            component.SubmitMessages.Enqueue(submitMessage);
        }
    }
}