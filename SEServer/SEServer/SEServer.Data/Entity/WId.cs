using MessagePack;

namespace SEServer.Data;

/// <summary>
/// 世界ID
/// </summary>
[MessagePackObject]
public struct WId
{
    [Key(0)]
    public int Id { get; set; }
    public static WId Invalid { get; } = new WId { Id = -1 };

    public override bool Equals(object? obj)
    {
        return obj is WId other && Equals(other);
    }

    public bool Equals(WId other)
    {
        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id;
    }
    
    public static bool operator ==(WId left, WId right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(WId left, WId right)
    {
        return !(left == right);
    }

    public override string ToString()
    {
        return $"WorldId: {Id}";
    }
}