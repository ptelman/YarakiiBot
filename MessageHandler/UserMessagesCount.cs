using System;
using YarakiiBot.Base;
using YarakiiBot.Model;

namespace YarakiiBot.MessageHandler{
    public class UserMessagesCount : IMessageReceiver, ICommandReceiver
    {
        public string HandleCommand(string username, string command)
        {
            if (command.StartsWith("!messages")){
                using(var context = new DatabaseContext()){
                    User user = context.GetUserByName(username);
                    int messages = user != null ? user.MessagesInChat : 0;

                    return $"User {username} sent {messages} messages in chat";
                }
            }
            return null;
        }

        public void HandleIncommingMessage(string user, string message)
        {
            using(var context = new DatabaseContext()){
                context.AddMessageToUser(user);
            }
        }
    }
}