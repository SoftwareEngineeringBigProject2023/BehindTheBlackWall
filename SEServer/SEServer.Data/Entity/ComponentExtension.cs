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

    public static void AddSubmitMessage(this ISubmitComponent submitComponent, SubmitData submitData)
    {
        submitComponent.SubmitMessages.Enqueue(submitData);
    }
    
    public static void AddNotifyMessage(this INotifyComponent notifyComponent, NotifyData notifyData)
    {
        notifyComponent.NotifyMessages.Enqueue(notifyData);
    }
    
    public static void ClearSubmitMessages(this ISubmitComponent submitComponent)
    {
        submitComponent.SubmitMessages.Clear();
    }
}