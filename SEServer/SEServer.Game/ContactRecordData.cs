using SEServer.Data;

namespace SEServer.Game;

public struct ContactRecordData
{
    public PhysicsData OtherPhysicsData { get; set; }
    /// <summary>
    /// 碰撞法线，由自身指向对方
    /// </summary>
    public SVector2 Normal { get; set; }
    public SVector2 ContactPoint { get; set; }
}