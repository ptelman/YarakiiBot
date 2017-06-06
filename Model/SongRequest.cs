using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace YarakiiBot.Model{
    public class SongRequest{
        public int SongRequestId { get; set; }
        public int UserId { get; set; }
        public string Link { get; set; }
    }
}