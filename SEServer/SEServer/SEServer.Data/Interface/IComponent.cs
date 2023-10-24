namespace SEServer.Data;

public interface IComponent
{
    public CId Id { get; set; }
    public EId EntityId { get; set; }
}