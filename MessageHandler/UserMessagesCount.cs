using System;
using YarakiiBot.Base;
using YarakiiBot.Model;

namespace YarakiiBot.MessageHandler{
    public class UserMessagesCount : IMessageReceiver
    {
        DatabaseContext databaseContext;
        public UserMessagesCount(DatabaseContext context){
            this.databaseContext = context;
        }

        public void HandleIncommingMessage(string user, string message)
        {
            databaseContext.AddMessageToUser(user);
        }
    }
}