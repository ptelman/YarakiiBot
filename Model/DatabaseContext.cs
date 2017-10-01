using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace YarakiiBot.Model{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<SongRequest> SongRequests { get; set; }
        public DbSet<ChatCommand> ChatCommands { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=yarakiiDatabase.db");
        }

        public User GetUserByName(string nickname){
            var task = Users.SingleOrDefaultAsync(u => u.Username == nickname);
            task.Wait();
            return task.Result;
        }

        public void RewardUsersForWatchingStream(IEnumerable<string> usernames){
            foreach(var username in usernames){
                this.AddPointsToUser(username, 5);
            }
            SaveChangesAsync();
        }

        public void AddPointsToUser(string nickname, int points){
            var task = Users.SingleOrDefaultAsync(u => u.Username == nickname);
            task.Wait();
            User user = task.Result;
            if (user == null){
                user = new User(){
                    Username = nickname,
                    MessagesInChat = 0,
                    Points = points
                };
                Users.Add(user);
            }
            else{
                user.Points += points;
                Users.Update(user);
            }
            SaveChanges();
        }

        public void AddMessageToUser(string nickname){
            var task = Users.SingleOrDefaultAsync(u => u.Username == nickname);
            task.Wait();
            User user = task.Result;
            if (user == null){
                user = new User(){
                    Username = nickname,
                    MessagesInChat = 1,
                    Points = 0
                };
                Users.Add(user);
            }
            else{
                user.MessagesInChat++;
                Users.Update(user);
            }
            SaveChanges();
        }

        public void UpdateChatCommand(string command, string response){
            var task = ChatCommands.SingleOrDefaultAsync(u => u.Command == command);
            task.Wait();
            ChatCommand chatCommand = task.Result;
            if (chatCommand == null){
                AddChatCommand(command, response);
            }
            else{
                chatCommand.Response = response;
                ChatCommands.Update(chatCommand);
                SaveChanges();
            }
        }

        private void AddChatCommand(string command, string response){
            ChatCommand chatCommand = new ChatCommand()
            {
                Command = command,
                Response = response
            };
            ChatCommands.Add(chatCommand);
            SaveChanges();
        }

        public void RemoveChatCommand(string command){
            var task = ChatCommands.SingleOrDefaultAsync(u => u.Command == command);
            task.Wait();
            ChatCommand chatCommand = task.Result;
            if (chatCommand != null){
                ChatCommands.Remove(chatCommand);
                SaveChanges();
            }
        }
    }
}