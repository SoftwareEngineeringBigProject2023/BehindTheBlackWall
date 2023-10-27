using System.Collections.Generic;

namespace SEServer.Data;

public static class ComponentExtension
{
    public static List<INotifyMessage> TakeAllNotifyMessages(this INotifyComponent component)
    {
        var messages = new List<INotifyMessage>();
        messages.AddRange(component.NotifyMessages);
        component.NotifyMessages.Clear();

        return messages;
    }
    
    public static void ReceiveNotifyMessages(this INotifyComponent component, List<INotifyMessage> messages)
    {
        foreach (var notifyMessage in messages)
        {
            component.NotifyMessages.Enqueue(notifyMessage);
        }
    }
    
    public static List<ISubmitMessage> TakeAllSubmitMessages(this ISubmitComponent component)
    {
        var messages = new List<ISubmitMessage>();
        messages.AddRange(component.SubmitMessages);
        component.SubmitMessages.Clear();

        return messages;
    }
    
    public static void ReceiveSubmitMessages(this ISubmitComponent component, List<ISubmitMessage> messages)
    {
        foreach (var submitMessage in messages)
        {
            component.SubmitMessages.Enqueue(submitMessage);
        }
    }
}