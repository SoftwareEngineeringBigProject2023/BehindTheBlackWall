using MessagePack;

namespace SEServer.Data;

[MessagePackObject]
public struct CId
{
    [Key(0)]
    public int Id { get; set; }
    public override bool Equals(object? obj)
    {
        return obj is CId other && Equals(other);
    }

    public bool Equals(CId other)
    {
        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id;
    }
    
    public static bool operator ==(CId left, CId right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(CId left, CId right)
    {
        return !(left == right);
    }
}