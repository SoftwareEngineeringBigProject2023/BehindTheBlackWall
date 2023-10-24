using SEServer.Data;

namespace SEServer.Core;

public struct UserId
{
    public int Id { get; set; }
    public static UserId Invalid { get; } = new UserId { Id = -1 };

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
    
    public static bool operator ==(UserId left, UserId right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(UserId left, UserId right)
    {
        return !(left == right);
    }
    
    public override string ToString()
    {
        return $"UserId: {Id}";
    }
}