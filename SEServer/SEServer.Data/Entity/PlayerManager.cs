using System.Collections.Generic;

namespace SEServer.Data;

public class PlayerManager
{
    public List<PlayerData> Players { get; set; } = new();
    public List<PlayerId> JoinedPlayers { get; set; } = new();
    public List<PlayerId> LeftPlayers { get; set; } = new();
    private int IdAutoIncrement { get; set; } = 1;
    
    private PlayerId CreateId()
    {
        PlayerId id = new PlayerId()
        {
            Id = IdAutoIncrement++
        };
        return id;
    }

    public PlayerData CreatePlayer()
    {
        var id = CreateId();
        return new PlayerData()
        {
            Id = id
        };
    }

    public PlayerData GetPlayer(PlayerId id)
    {
        return Players.Find(x => x.Id == id);
    }
    
    public void RemovePlayer(PlayerId id)
    {
        var player = Players.Find(x => x.Id == id);
        if (player != null)
        {
            Players.Remove(player);
            LeftPlayers.Add(id);
        }
    }
    
    public void Clear()
    {
        Players.Clear();
        JoinedPlayers.Clear();
        LeftPlayers.Clear();
    }
}