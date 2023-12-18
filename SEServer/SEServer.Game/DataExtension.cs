namespace SEServer.Game;

public static class DataExtension 
{
    public static T? TryGet<T>(this List<T> list, int index)
    {
        if (index < 0 || index >= list.Count)
        {
            return default;
        }

        return list[index];
    }
}