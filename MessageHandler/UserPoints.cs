using System;
using YarakiiBot.Base;
using YarakiiBot.Model;

namespace YarakiiBot.MessageHandler{
    public class UserPoints : ICommandReceiver
    {
        public string HandleCommand(string username, string command)
        {
            if (command.StartsWith("!points")){
                using (var context = new DatabaseContext()){
                    User user = context.GetUserByName(username);
                    int points = user != null ? user.Points : 0;

                    return $"User {username} has {points} points";
                }
            }
            return null;
        }
    }
}