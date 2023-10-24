namespace SEServer.Core;

public class UserManager
{
    public List<User> Users { get; set; } = new();
    
    public User? GetUser(UserId id)
    {
        return Users.FirstOrDefault(u => u.Id == id);
    }
    
    public User CreateOrGetUser(UserId id)
    {
        var user = GetUser(id);
        if (user is null)
        {
            user = new User
            {
                Id = id
            };
            Users.Add(user);
        }
        return user;
    }
    
    public void RemoveUser(UserId id)
    {
        var user = GetUser(id);
        if (user != null)
        {
            Users.Remove(user);
        }
    }
}