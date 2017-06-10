using System.Collections.Generic;

namespace YarakiiBot.Base{
    public class Settings{
        public string Username { get; set; }
        public string AccessToken { get; set; }
        public IEnumerable<string> PrivilegedUsers { get; set; }
        public string Channel { get; set; }
        public string ClientId { get; set; }
        public string ChannelId { get; set; }
        public string ClientSecret { get; set; }
    }
}