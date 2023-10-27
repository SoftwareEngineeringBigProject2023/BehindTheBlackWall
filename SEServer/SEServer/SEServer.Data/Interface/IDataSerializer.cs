namespace SEServer.Data;

public interface IDataSerializer : IService
{
    /// <summary>
    /// 返回data的序列化后的bytes
    /// </summary>
    /// <param name="data"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    byte[] Serialize<T>(T data);
    /// <summary>
    /// 从bytes中的offset位置开始，将data序列化到bytes中
    /// </summary>
    /// <param name="data"></param>
    /// <param name="bytes"></param>
    /// <param name="offset"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    int Serialize<T>(T data, byte[] bytes, int offset);
    T Deserialize<T>(byte[] bytes, int offset, int size);
}