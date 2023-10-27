namespace SEServer.Data;

public class ComponentDataPack<T> where T : IComponent
{
    public CId Id { get; set; }
    public T Component { get; set; }
}