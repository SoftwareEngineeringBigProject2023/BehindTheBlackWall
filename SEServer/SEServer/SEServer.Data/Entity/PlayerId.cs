namespace SEServer.Data;

public struct PlayerId
{
    public int Id { get; set; }
    public static PlayerId Invalid { get; } = new PlayerId { Id = -1 };

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
    
    public static bool operator ==(PlayerId left, PlayerId right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(PlayerId left, PlayerId right)
    {
        return !(left == right);
    }
}