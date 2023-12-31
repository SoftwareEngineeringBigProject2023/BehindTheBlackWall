﻿using System;
using System.Collections.Generic;

namespace SEServer.Data.Interface;

public interface IComponentArray
{
    World World { get; set; }
    void RemoveMarkComponents();
    void MarkAsToBeDelete(EId eId);
    /// <summary>
    /// 标记为脏，需要进行网络同步
    /// </summary>
    /// <param name="component"></param>
    void MarkAsDirty(INetComponent component);
    bool IsDirty(INetComponent component);
    void ClearDirty();
    /// <summary>
    /// 标记为改变，需要进行System处理
    /// </summary>
    /// <param name="component"></param>
    void MarkAsChanged(IComponent component);
    bool IsChanged(IComponent component);
    void ClearChanged();
    bool ContainInterface(Type iType);
    Type GetDataType();
    bool HasEntity(EId entityId);
    IComponent GetI(EId entityId);
    IEnumerable<IComponent> GetAllComponents();
    ComponentArrayDataPack WriteToDataPack(IComponentSerializer serializer);
    void ReadFromDataPack(IComponentSerializer serializer, ComponentArrayDataPack dataPack);
    ComponentArrayDataPack? WriteChangedToDataPack(IComponentSerializer serializer, List<EId> includeEIds, PlayerId playerId);
    void ReadChangedFromDataPack(IComponentSerializer serializer, ComponentArrayDataPack dataPack);
    /// <summary>
    /// 仅对 INotifyComponent 有效
    /// </summary>
    /// <param name="serializer"></param>
    /// <param name="includeEIds">额外包含的实体</param>
    /// <returns></returns>
    ComponentNotifyMessageDataPack? WriteChangedToNotifyDataPack(IComponentSerializer serializer, List<EId> includeEIds);
    /// <summary>
    /// 仅对 INotifyComponent 有效
    /// </summary>
    /// <param name="serializer"></param>
    /// <param name="dataPack"></param>
    void ReadChangedFromNotifyDataPack(IComponentSerializer serializer, ComponentNotifyMessageDataPack dataPack);

    /// <summary>
    /// 仅对 ISubmitComponent 有效
    /// </summary>
    /// <param name="serializer"></param>
    /// <param name="includeEIds">额外包含的实体</param>
    /// <param name="player"></param>
    /// <returns></returns>
    ComponentSubmitMessageDataPack? WriteChangedToSubmitDataPack(IComponentSerializer serializer, List<EId> includeEIds, PlayerId player);
    /// <summary>
    /// 仅对 ISubmitComponent 有效
    /// </summary>
    /// <param name="serializer"></param>
    /// <param name="dataPack"></param>
    void ReadChangedFromSubmitDataPack(IComponentSerializer serializer, ComponentSubmitMessageDataPack dataPack);
    /// <summary>
    /// 服务器传输空Submit组件时使用
    /// </summary>
    /// <param name="serializer"></param>
    /// <param name="allEntitiesChanged"></param>
    /// <returns></returns>
    ComponentArrayDataPack? WriteEmptySubmitToDataPack(IComponentSerializer serializer, List<EId> allEntitiesChanged);
}