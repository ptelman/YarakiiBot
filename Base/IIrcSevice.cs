namespace YarakiiBot.Base{
    public interface IIrcService
    {
        void SendMessage(string message);
        void Subscribe(IMessageReceiver messageReceiver);
        void Unsubscribe(IMessageReceiver messageReceiver);
    }
}