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
    }
}