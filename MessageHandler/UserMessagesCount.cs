using System;
using YarakiiBot.Base;
using YarakiiBot.Model;

namespace YarakiiBot.MessageHandler{
    public class UserMessagesCount : IMessageReceiver, ICommandReceiver
    {
        DatabaseContext databaseContext;
        public UserMessagesCount(DatabaseContext context){
            this.databaseContext = context;
        }

        public string HandleCommand(string username, string command)
        {
            if (command.StartsWith("!messages")){
                User user = databaseContext.GetUserByName(username);
                int messages = user != null ? user.MessagesInChat : 0;

                return $"User {username} sent {messages} messages in chat";
            }
            return null;
        }

        public void HandleIncommingMessage(string user, string message)
        {
            databaseContext.AddMessageToUser(user);
        }
    }
}