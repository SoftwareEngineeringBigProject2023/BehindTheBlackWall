using MessagePack;

namespace SEServer.Data;

[MessagePackObject]
public struct EId
{
    [Key(0)]
    public int Id { get; set; }
    public static EId Invalid { get; } = new EId { Id = -1 };

    public override bool Equals(object? obj)
    {
        return obj is EId other && Equals(other);
    }

    public bool Equals(EId other)
    {
        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id;
    }
    
    public static bool operator ==(EId left, EId right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(EId left, EId right)
    {
        return !(left == right);
    }
}