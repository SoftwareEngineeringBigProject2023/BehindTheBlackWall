using System;
using System.Collections.Generic;

namespace SEServer.Data;

public class ClientWorld : World
{
    public void StartUp(Snapshot snapshot)
    {
        Id = snapshot.WorldId;
        EntityManager.ApplySnapshot(snapshot);
    }

    public PlayerId PlayerId { get; set; }
    public Queue<IWorldMessage> SendMessageQueue { get; } = new Queue<IWorldMessage>();
    public Queue<IWorldMessage> ReceiveMessageQueue { get; } = new Queue<IWorldMessage>();
    
    public override void Update(float deltaTime)
    {
        try
        {
            Time.Update(deltaTime);
            EntitiesUpdate();
        }
        catch (Exception e)
        {
            ServerContainer.Get<ILogger>().LogError(e.ToString());
        }
    }

    private void EntitiesUpdate()
    {
        var Entities = EntityManager.Entities;
        var Components = EntityManager.Components;
        var Systems = EntityManager.Systems;
        
        // 从消息队列收集信息，应用实体信息变动
        ReceiveMessage();
        
        // 更新System
        Systems.UpdateAll();
            
        // 收集所有变动的实体信息，传入消息队列
        CollectChangedInfo();
            
        // 移除所有标记的组件
        Entities.ClearDirty();
        Components.ClearDirty();
    }

    private void ReceiveMessage()
    {
        while (ReceiveMessageQueue.Count > 0)
        {
            var message = ReceiveMessageQueue.Dequeue();
            switch (message)
            {
                case SyncEntityMessage syncEntityMessage:
                    ApplySyncEntityMessage(syncEntityMessage);
                    break;
                default:
                    ServerContainer.Get<ILogger>().LogError($"未知消息类型：{message.GetType()}");
                    break;
            }
        }
    }
    
    private void CollectChangedInfo()
    {
        var submitEntityMessage = new SubmitEntityMessage();

        var serializer = ServerContainer.Get<IComponentSerializer>();
        var allEntitiesChanged = EntityManager.Entities.GetChangedEntityIds();
        
        // 收集所有变动的组件信息，传入消息队列
        // 记录组件变动 实体变动或组件变动 都会记录
        foreach (var pair in EntityManager.Components.Components)
        {
            var componentArray = pair.Value;
            if (componentArray.ContainInterface(typeof(ISubmitComponent)))
            {
                var componentNotifyDataPack = componentArray.WriteChangedToSubmitDataPack(serializer, allEntitiesChanged, PlayerId);
                submitEntityMessage.ComponentSubmitDataPacks.Add(componentNotifyDataPack);
            }
            else if (componentArray.ContainInterface(typeof(IC2SComponent)))
            {
                var componentArrayDataPack = componentArray.WriteChangedToDataPack(serializer, allEntitiesChanged, PlayerId);
                submitEntityMessage.ComponentArrayDataPacks.Add(componentArrayDataPack);
            }
        }
        
        // 收集完毕，传入消息队列
        if (submitEntityMessage.ComponentArrayDataPacks.Count > 0 || submitEntityMessage.ComponentSubmitDataPacks.Count > 0)
        {
            SendMessageQueue.Enqueue(submitEntityMessage);
        }
    }

    private void ApplySyncEntityMessage(SyncEntityMessage syncEntityMessage)
    {
        var components = EntityManager.Components;
        
        // 添加实体
        foreach (var entityId in syncEntityMessage.EntitiesToCreate)
        {
            EntityManager.CreateEntity(entityId);
        }
        
        // 删除实体
        foreach (var entityId in syncEntityMessage.EntitiesToDelete)
        {
            EntityManager.RemoveEntity(entityId);
        }
        
        EntityManager.Entities.RemoveMarkEntities();
        
        // 更新组件
        var serializer = ServerContainer.Get<IComponentSerializer>();
        foreach (var arrayDataPack in syncEntityMessage.ComponentArrayDataPacks)
        {
            var dataType = serializer.GetTypeByCode(arrayDataPack.TypeCode);
            var componentArray = components.GetComponentArray(dataType);
            componentArray.ReadChangedFromDataPack(serializer, arrayDataPack);
        }
        
        // 更新消息
        foreach (var notifyDataPack in syncEntityMessage.ComponentNotifyDataPacks)
        {
            var dataType = serializer.GetTypeByCode(notifyDataPack.TypeCode);
            var componentArray = components.GetComponentArray(dataType);
            componentArray.ReadChangedFromNotifyDataPack(serializer, notifyDataPack);
        }
    }
}