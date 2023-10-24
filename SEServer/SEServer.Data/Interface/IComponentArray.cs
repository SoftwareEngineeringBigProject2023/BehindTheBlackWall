using System;
using System.Collections.Generic;

namespace SEServer.Data;

public interface IComponentArray
{
    void RemoveMarkComponents();
    void MarkAsToBeDelete(EId eId);
    void ClearDirty();
    bool ContainInterface(Type iType);
    Type GetDataType();
    ComponentArrayDataPack WriteToDataPack(IComponentSerializer serializer);
    void ReadFromDataPack(IComponentSerializer serializer, ComponentArrayDataPack dataPack);
    ComponentArrayDataPack WriteChangedToDataPack(IComponentSerializer serializer, List<EId> includeEIds, PlayerId playerId);
    void ReadChangedFromDataPack(IComponentSerializer serializer, ComponentArrayDataPack dataPack);
    /// <summary>
    /// 仅对 INotifyComponent 有效
    /// </summary>
    /// <param name="serializer"></param>
    /// <param name="includeEIds">额外包含的实体</param>
    /// <returns></returns>
    ComponentNotifyMessageDataPack WriteChangedToNotifyDataPack(IComponentSerializer serializer, List<EId> includeEIds);
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
    ComponentSubmitMessageDataPack WriteChangedToSubmitDataPack(IComponentSerializer serializer, List<EId> includeEIds, PlayerId player);
    /// <summary>
    /// 仅对 ISubmitComponent 有效
    /// </summary>
    /// <param name="serializer"></param>
    /// <param name="dataPack"></param>
    void ReadChangedFromSubmitDataPack(IComponentSerializer serializer, ComponentSubmitMessageDataPack dataPack);
}