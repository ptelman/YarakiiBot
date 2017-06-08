namespace YarakiiBot.Base{
    public interface IIrcManager
    {
        void SendMessage(string message);
        void Subscribe(IMessageReceiver messageReceiver);
        void Unsubscribe(IMessageReceiver messageReceiver);
    }
}