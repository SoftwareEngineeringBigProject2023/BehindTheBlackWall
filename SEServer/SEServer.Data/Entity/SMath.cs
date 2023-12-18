namespace SEServer.Data;

public static class SMath
{
    /// <summary>
    /// 弧度转角度
    /// </summary>
    /// <param name="rad"></param>
    /// <returns></returns>
    public static float Rad2Deg(float rad) => rad * 57.29578f;
    /// <summary>
    /// 角度转弧度
    /// </summary>
    /// <param name="deg"></param>
    /// <returns></returns>
    public static float Deg2Rad(float deg) => deg * 0.01745329f;
}