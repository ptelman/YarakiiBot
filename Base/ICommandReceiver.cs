namespace YarakiiBot.Base{
    public interface ICommandReceiver{
        string HandleCommand(string user, string command);
    }
}