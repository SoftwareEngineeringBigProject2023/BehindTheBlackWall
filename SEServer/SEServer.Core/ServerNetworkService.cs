using System.Net;
using System.Net.WebSockets;
using System.Text;
using Cysharp.Threading.Tasks;
using SEServer.Data;
using SEServer.Data.Interface;

namespace SEServer.Core;

public class ServerNetworkService : IServerNetworkService
{
    public ServerContainer ServerContainer { get; set; }
    public List<ClientConnect> ClientConnects { get; } = new();

    private bool _endFlog = false;

    public void Init()
    {
        
    }

    public void Start()
    {
        StartServer();
    }

    public void Stop()
    {
        EndServer();
    }
    
    public void StartServer()
    {
        UniTask.Run(RunServer);
    }
    
    public void EndServer()
    {
        _endFlog = true;
    }
    
    private async void RunServer()
    {
        const int port = 8080;
        var listener = new HttpListener();
        listener.Prefixes.Add($"http://localhost:{port}/Game/");
        listener.Start();
        
        while (true)
        {
            var context = await listener.GetContextAsync();
            if (context.Request.IsWebSocketRequest)
            {
                ServerContainer.Get<ILogger>().LogInfo("新的客户端连接");
                var client = new ClientConnect(context, ServerContainer);
                client.Start();
                lock (ClientConnects)
                {
                    ClientConnects.Add(client);
                }
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.Close();
            }
            
            if(_endFlog)
                break;
        }
        
        listener.Stop();
    }
    
    public void RemoveUnconnectedClient()
    {
        lock (ClientConnects)
        {
            for (var index = 0; index < ClientConnects.Count; index++)
            {
                var connect = ClientConnects[index];
                if (connect.State == ClientConnectState.Disconnected)
                {
                    ClientConnects.RemoveAt(index);
                    index--;
                }
            }
        }
    }
}