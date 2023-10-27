using MessagePack;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace SEServer.Data;

[MessagePackObject()]
public struct SVector2
{
    [Key(0)] public float X;
    [Key(1)] public float Y;
    
    public static SVector2 Zero => new(0, 0);
    public static SVector2 One => new(1, 1);
    public static SVector2 Invalid => new(float.NaN, float.NaN);

    public SVector2(float vector2X, float vector2Y)
    {
        X = vector2X;
        Y = vector2Y;
    }

    public override bool Equals(object? obj)
    {
        return obj is SVector2 vector2 && Equals(vector2);
    }
    
    public bool Equals(SVector2 other)
    {
        if(float.IsNaN(X) && float.IsNaN(other.X) && float.IsNaN(Y) && float.IsNaN(other.Y))
            return true;
        
        return X == other.X && Y == other.Y;
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }

    public override int GetHashCode()
    {
        return X.GetHashCode() ^ Y.GetHashCode();
    }
    
    public static bool operator ==(SVector2 left, SVector2 right)
    {
        return left.Equals(right);
    }
    
    public static bool operator !=(SVector2 left, SVector2 right)
    {
        return !(left == right);
    }
    
    public static SVector2 operator +(SVector2 left, SVector2 right)
    {
        return new(left.X + right.X, left.Y + right.Y);
    }
    
    public static SVector2 operator -(SVector2 left, SVector2 right)
    {
        return new(left.X - right.X, left.Y - right.Y);
    }
    
    public static SVector2 operator *(SVector2 left, float right)
    {
        return new(left.X * right, left.Y * right);
    }
    
    public static SVector2 operator *(float left, SVector2 right)
    {
        return new(left * right.X, left * right.Y);
    }
}