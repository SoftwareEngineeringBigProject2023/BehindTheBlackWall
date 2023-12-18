using System;
using MessagePack;
using System.Numerics;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace SEServer.Data;

[MessagePackObject()]
public struct SVector2
{
    [Key(0)] 
    public float X;
    
    [Key(1)] 
    public float Y;
    
    public static SVector2 Zero => new(0, 0);
    public static SVector2 One => new(1, 1);
    public static SVector2 Up => new(0, 1);
    public static SVector2 Down => new(0, -1);
    public static SVector2 Left => new(-1, 0);
    public static SVector2 Right => new(1, 0);
    public static SVector2 Invalid => new(float.NaN, float.NaN);
    [IgnoreMember]
    public float magnitude => Length();
    [IgnoreMember]
    public SVector2 normalized => Normalize(this);
    
    public static SVector2 Normalize(SVector2 vector2)
    {
        var length = vector2.Length();
        if (length == 0)
        {
            return Zero;
        }
        else
        {
            return new SVector2(vector2.X / length, vector2.Y / length);
        }
    }

    public static float Distance(SVector2 a, SVector2 b)
    {
        return (a - b).Length();
    }

    public float Length()
    {
        return (float)Math.Sqrt(X * X + Y * Y);
    }

    public float LengthSquared()
    {
        return X * X + Y * Y;
    }

    public float Angle()
    {
        return SMath.Rad2Deg((float)Math.Atan2(Y, X));
    }

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
        unchecked
        {
            int hash = 17; // 选择一个初始哈希值，通常是一个质数

            // 将 X 和 Y 转换为整数并组合它们的哈希码
            hash = hash * 23 + X.GetHashCode();
            hash = hash * 23 + Y.GetHashCode();

            return hash;
        }
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
        return new SVector2(left.X + right.X, left.Y + right.Y);
    }
    
    public static SVector2 operator -(SVector2 left, SVector2 right)
    {
        return new SVector2(left.X - right.X, left.Y - right.Y);
    }
    
    public static SVector2 operator *(SVector2 left, float right)
    {
        return new SVector2(left.X * right, left.Y * right);
    }
    
    public static SVector2 operator *(float left, SVector2 right)
    {
        return new SVector2(left * right.X, left * right.Y);
    }
    
    public static SVector2 operator -(SVector2 vector2)
    {
        return new SVector2(-vector2.X, -vector2.Y);
    }

    /// <summary>
    /// 从角度制创建
    /// </summary>
    /// <param name="angle">角度制</param>
    /// <returns></returns>
    public static SVector2 FromAngle(float angle)
    {
        var rad = SMath.Deg2Rad(angle);
        
        // 右为0度，逆时针为正
        return new SVector2((float)Math.Cos(rad), (float)Math.Sin(rad));
    }
    
    /// <summary>
    /// 绕原点旋转
    /// </summary>
    /// <param name="angle">角度制</param>
    /// <returns></returns>
    public SVector2 Rotate(float angle)
    {
        var rad = SMath.Deg2Rad(angle);
        
        var cosTheta = (float)Math.Cos(rad);
        var sinTheta = (float)Math.Sin(rad);

        var xRotated = X * cosTheta - Y * sinTheta;
        var yRotated = X * sinTheta + Y * cosTheta;

        return new SVector2(xRotated, yRotated);
    }

    public static SVector2 Parse(string str)
    {
        var sepPos = str.IndexOf(',');
        if (sepPos == -1)
        {
            throw new FormatException("Invalid SVector2 format.");
        }

        string xStr;
        if (str.StartsWith("("))
        {
            xStr = str.Substring(1, sepPos - 1);
        }
        else
        {
            xStr = str.Substring(0, sepPos - 1);
        }

        xStr = xStr.Trim();
        
        string yStr;
        if (str.EndsWith(")"))
        {
            yStr = str.Substring(sepPos + 1, str.Length - sepPos - 2);
        }
        else
        {
            yStr = str.Substring(sepPos + 1, str.Length - sepPos - 1);
        }
        
        yStr = yStr.Trim();
        
        if (!float.TryParse(xStr, out var x))
        {
            throw new FormatException("Invalid SVector2 x format.");
        }
        
        if (!float.TryParse(yStr, out var y))
        {
            throw new FormatException("Invalid SVector2 y format.");
        }
        
        return new SVector2(x, y);
    }

    /// <summary>
    /// 两个向量的点积
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float Dot(SVector2 a, SVector2 b)
    {
        return (a.X * b.X) + (a.Y * b.Y);
    }
    
    /// <summary>
    /// 两个向量的夹角
    /// </summary>
    /// <param name="vectorA"></param>
    /// <param name="vectorB"></param>
    /// <returns></returns>
    public static float Angle(SVector2 vectorA, SVector2 vectorB)
    {
        float dotProduct = Dot(vectorA, vectorB);
        float magnitudeA = vectorA.Length();
        float magnitudeB = vectorB.Length();

        // 计算余弦值
        float cosTheta = dotProduct / (magnitudeA * magnitudeB);

        // 使用反余弦函数计算夹角（弧度）
        float angleRadians = (float)Math.Acos(cosTheta);

        return SMath.Rad2Deg(angleRadians);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="normal"></param>
    /// <returns></returns>
    public static SVector2 Reflect(SVector2 vector, SVector2 normal)
    {
        return vector - 2 * Dot(vector, normal) * normal;
    }
}