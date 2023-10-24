using System.Collections.Generic;

namespace SEServer.Data;

public class PlayerManager
{
    public List<PlayerData> Users { get; set; } = new();
    public List<PlayerId> JoinedPlayers { get; set; } = new();
    public List<PlayerId> LeftPlayers { get; set; } = new();

    public PlayerData CreateOrGetPlayer(PlayerId id)
    {
        var player = Users.Find(x => x.Id == id);
        if (player == null)
        {
            player = new PlayerData
            {
                Id = id
            };
            Users.Add(player);
            JoinedPlayers.Add(id);
        }
        
        return player;
    }
    
    public void RemovePlayer(PlayerId id)
    {
        var player = Users.Find(x => x.Id == id);
        if (player != null)
        {
            Users.Remove(player);
            LeftPlayers.Add(id);
        }
    }
    
    public void Clear()
    {
        Users.Clear();
        JoinedPlayers.Clear();
        LeftPlayers.Clear();
    }
}