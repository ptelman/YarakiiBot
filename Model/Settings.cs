using System.Collections.Generic;

namespace YarakiiBot.Base{
    public class Settings{
        public string Username { get; set; }
        public string Password { get; set; }
        public IEnumerable<string> PrivilegedUsers { get; set; }
        public string Channel { get; set; }
    }
}