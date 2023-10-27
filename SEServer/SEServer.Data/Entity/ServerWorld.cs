using System;
using System.Collections.Generic;
using System.Linq;
using SEServer.Data.Interface;
using SEServer.Data.Message;

namespace SEServer.Data;

public class ServerWorld : World
{
    private const long SNAPSHOT_INTERVAL = 10;

    public void StartUp(WId wId)
    {
        this.Id = wId;
    }
    
    public Snapshot NearestSnapshot { get; set; } = null!;
    public Queue<IWorldMessage> SendMessageQueue { get; } = new Queue<IWorldMessage>();
    public Queue<IWorldMessage> ReceiveMessageQueue { get; } = new Queue<IWorldMessage>();
    public PlayerManager PlayerManager { get; } = new PlayerManager();
    /// <summary>
    /// 增量状态信息
    /// </summary>
    public List<IWorldMessage> IncrementalStateInfo { get; } = new List<IWorldMessage>();

    public override void Update(float deltaTime)
    {
        try
        {
            Time.Update(deltaTime);
            EntitiesUpdate();

            if(Time.FrameCount % SNAPSHOT_INTERVAL == 0)
            {
                // 生成快照
                var snapshot = EntityManager.GetSnapshot();
                NearestSnapshot = snapshot;
                IncrementalStateInfo.Clear();
            }
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
        
        // 获取所有消息，更新实体
        ReceiveMessage();
        
        // 更新System
        Systems.UpdateAll();
            
        // 移除所有标记的组件
        Components.MarkAsToBeDeleteAll(Entities.DeleteEntities);
        Components.RemoveMarkComponents();
        
        // 移除所有标记的实体
        Entities.RemoveMarkEntities();

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
                case SubmitEntityMessage submitEntityMessage:
                    ApplySubmitEntityMessage(submitEntityMessage);
                    break;
                default:
                    ServerContainer.Get<ILogger>().LogError($"未知消息类型：{message.GetType()}");
                    break;
            }
        }
    }

    private void CollectChangedInfo()
    {
        var Entities = EntityManager.Entities;
        var allEntitiesChanged = Entities.GetChangedEntityIds();
        
        var syncEntityMessage = new SyncEntityMessage();
        // 先收集实体的删改信息
        syncEntityMessage.EntitiesToDelete.AddRange(EntityManager.Entities.DeleteEntities);
        syncEntityMessage.EntitiesToCreate.AddRange(EntityManager.Entities.CreateEntities);
        var serializer = ServerContainer.Get<IComponentSerializer>();
        // 记录组件变动 实体变动或组件变动 都会记录
        foreach (var pair in EntityManager.Components.Components)
        {
            var componentArray = pair.Value;
            if (componentArray.ContainInterface(typeof(INotifyComponent)))
            {
                var componentNotifyDataPack = componentArray.WriteChangedToNotifyDataPack(serializer, allEntitiesChanged);
                if (componentNotifyDataPack != null)
                {
                    syncEntityMessage.ComponentNotifyDataPacks.Add(componentNotifyDataPack);
                }
            }
            else if (componentArray.ContainInterface(typeof(IS2CComponent)) || componentArray.ContainInterface(typeof(IC2SComponent)))
            {
                var componentArrayDataPack = componentArray.WriteChangedToDataPack(serializer, allEntitiesChanged, PlayerId.Invalid);
                if (componentArrayDataPack != null)
                {
                    syncEntityMessage.ComponentArrayDataPacks.Add(componentArrayDataPack);
                }
            }
            else if (componentArray.ContainInterface(typeof(ISubmitComponent)))
            {
                // 给客户端的空提交组件
                var componentArrayDataPack = componentArray.WriteEmptySubmitToDataPack(serializer, allEntitiesChanged);
                if (componentArrayDataPack != null)
                {
                    syncEntityMessage.ComponentArrayDataPacks.Add(componentArrayDataPack);
                }
            }
        }
        // 收集完毕，传入消息队列
        if (syncEntityMessage.EntitiesToDelete.Count > 0 
            || syncEntityMessage.EntitiesToCreate.Count > 0
            || syncEntityMessage.ComponentNotifyDataPacks.Count > 0 
            || syncEntityMessage.ComponentArrayDataPacks.Count > 0)
        {
            SendMessageQueue.Enqueue(syncEntityMessage);
            IncrementalStateInfo.Add(syncEntityMessage);
        }
    }
    
    private void ApplySubmitEntityMessage(SubmitEntityMessage submitEntityMessage)
    {
        var components = EntityManager.Components;

        // 更新组件
        var serializer = ServerContainer.Get<IComponentSerializer>();
        foreach (var arrayDataPack in submitEntityMessage.ComponentArrayDataPacks)
        {
            var dataType = serializer.GetTypeByCode(arrayDataPack.TypeCode);
            var componentArray = components.GetComponentArray(dataType);
            componentArray.ReadChangedFromDataPack(serializer, arrayDataPack);
        }
        
        // 更新消息
        foreach (var submitDataPack in submitEntityMessage.ComponentSubmitDataPacks)
        {
            var dataType = serializer.GetTypeByCode(submitDataPack.TypeCode);
            var componentArray = components.GetComponentArray(dataType);
            componentArray.ReadChangedFromSubmitDataPack(serializer, submitDataPack);
        }
    }
}