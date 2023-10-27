using System.Collections.Generic;

namespace SEServer.Data;

public static class ComponentExtension
{
    public static List<NotifyMessage> TakeAllNotifyMessages(this INotifyComponent component)
    {
        var messages = new List<NotifyMessage>();
        messages.AddRange(component.NotifyMessages);
        component.NotifyMessages.Clear();

        return messages;
    }
    
    public static void ReceiveNotifyMessages(this INotifyComponent component, List<NotifyMessage> messages)
    {
        foreach (var notifyMessage in messages)
        {
            component.NotifyMessages.Enqueue(notifyMessage);
        }
    }
    
    public static List<SubmitMessage> TakeAllSubmitMessages(this ISubmitComponent component)
    {
        var messages = new List<SubmitMessage>();
        messages.AddRange(component.SubmitMessages);
        component.SubmitMessages.Clear();

        return messages;
    }
    
    public static void ReceiveSubmitMessages(this ISubmitComponent component, List<SubmitMessage> messages)
    {
        foreach (var submitMessage in messages)
        {
            component.SubmitMessages.Enqueue(submitMessage);
        }
    }
}