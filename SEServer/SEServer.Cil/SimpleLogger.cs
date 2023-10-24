using SEServer.Data;

namespace SEServer.Cil;

public class SimpleLogger : ILogger
{
    public ServerContainer ServerContainer { get; set; }
    public void Init()
    {
        
    }

    public void Start()
    {
        
    }

    public void Stop()
    {
        
    }

    public void LogInfo(object msg)
    {
        Console.WriteLine(msg);
    }

    public void LogWarning(object msg)
    {
        using (new ConsoleColorSet(ConsoleColor.Yellow))
        {
            Console.WriteLine(msg);
        }
    }

    public void LogError(object msg)
    {
        using (new ConsoleColorSet(ConsoleColor.Red))
        {
            Console.WriteLine(msg);
        }
    }
}