using MessagePack;

namespace SEServer.Data;

[MessagePackObject()]
public struct SVector2
{
    [Key(0)] public float X;
    [Key(1)] public float Y;

    public SVector2(float vector2X, float vector2Y)
    {
        X = vector2X;
        Y = vector2Y;
    }
}