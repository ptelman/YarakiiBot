using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace YarakiiBot.Model{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<SongRequest> SongRequests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=yarakiiDatabase.db");
        }

        public User GetUserByName(string nickname){
            var task = Users.SingleOrDefaultAsync(u => u.Username == nickname);
            task.Wait();
            return task.Result;
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
    }
}