namespace SEServer.Cil;

public struct ConsoleColorSet : IDisposable
{
    public ConsoleColor OldForegroundColor { get; set; }
    public ConsoleColor OldBackgroundColor { get; set; }
    
    public ConsoleColorSet(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
    {
        OldForegroundColor = Console.ForegroundColor;
        OldBackgroundColor = Console.BackgroundColor;
        Console.ForegroundColor = foregroundColor;
        Console.BackgroundColor = backgroundColor;
    }
    
    public ConsoleColorSet(ConsoleColor foregroundColor)
    {
        OldForegroundColor = Console.ForegroundColor;
        OldBackgroundColor = Console.BackgroundColor;
        Console.ForegroundColor = foregroundColor;
    }
    
    public void Dispose()
    {
        Console.ForegroundColor = OldForegroundColor;
        Console.BackgroundColor = OldBackgroundColor;
    }
}