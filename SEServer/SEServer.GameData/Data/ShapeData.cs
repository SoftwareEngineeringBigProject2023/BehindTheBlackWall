using System.Collections.Generic;
using SEServer.Data;

namespace SEServer.GameData.Data;

public abstract class ShapeData
{
    public static int IdCounter = 0;

    public ShapeData()
    {
        Id = IdCounter++;
    }
    
    public int Id { get; set; }
    public bool IsChanged { get; set; }
    public SVector2 Position { get; set; }
    public float Rotation { get; set; }
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

public class RectangleShapeData : ShapeData
{
    public RectangleShapeData(float width, float height)
    {
        Width = width;
        Height = height;
    }

    private float _width;
    private float _height;

    public float Width
    {
        get => _width;
        set
        {
            _width = value;
            IsChanged = true;
        }
    }

    public float Height
    {
        get => _height;
        set
        {
            _height = value;
            IsChanged = true;
        }
    }
}

public class ChainShapeData : ShapeData
{
    public ChainShapeData(IEnumerable<SVector2> lines)
    {
        Lines.AddRange(lines);
    }
    
    public List<SVector2> Lines { get; } = new List<SVector2>();
}