
using System;
using System.Collections.Generic;
using System.IO;
using YarakiiBot.Base;
using YarakiiBot.Model;

namespace YarakiiBot.Service{
    public sealed class BasicCommandCache
    {
        private static volatile BasicCommandCache instance;
        private static object syncRoot = new Object();
        public List<ChatCommand> Commands {get; private set; }
        private BasicCommandCache() {
            Commands = ReadBasicCommands();
        }

        private List<ChatCommand> ReadBasicCommands(){
            var commands = new List<ChatCommand>();
            using (var context = new DatabaseContext()){
                commands.AddRange(context.ChatCommands);
            }
            return commands;
        }

        public void Update()
        {
            lock (syncRoot)
                Commands = ReadBasicCommands();           
        }

        public static BasicCommandCache Instance{
            get
            {
                if (instance == null){
                    lock (syncRoot){
                        if (instance == null) 
                            instance = new BasicCommandCache();
                    }
                }
                return instance;
            }
        }
    }
}