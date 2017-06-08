namespace YarakiiBot.Base{
    public interface IMessageReceiver{
        void HandleIncommingMessage(string user, string message);
    }
}