namespace SEServer.GameData;

public abstract class ShapeData
{
    public static int IdCounter = 0;

    public ShapeData()
    {
        Id = IdCounter++;
    }
    
    public int Id { get; set; }
    public bool IsChanged { get; set; }
}

public class CircleShapeData : ShapeData
{
    public CircleShapeData(float radius)
    {
        Radius = radius;
    }

    private float _radius;

    public float Radius
    {
        get => _radius;
        set
        {
            _radius = value;
            IsChanged = true;
        }
    }
}