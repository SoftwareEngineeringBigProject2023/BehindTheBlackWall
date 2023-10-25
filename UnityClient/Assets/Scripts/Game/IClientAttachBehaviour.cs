namespace Game
{
    public interface IClientAttachBehaviour
    {
        ClientBehaviour ClientBehaviour { get; set; }
        
        void UpdateBehaviour();
    }
}