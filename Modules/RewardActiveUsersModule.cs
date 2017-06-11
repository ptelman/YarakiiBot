using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TwitchLib;
using TwitchLib.Models.API;
using YarakiiBot.Base;
using YarakiiBot.Model;

namespace YarakiiBot.Modules{
    public class RewardActiveUsersModule : IModule
    {
        private Settings settings;
        private DatabaseContext databaseContext;
        private Task rewardEvery5MinutesTask;

        public RewardActiveUsersModule(IOptions<Settings> settings, DatabaseContext databaseContext)
        {
            this.settings = settings.Value;
            this.databaseContext = databaseContext;
        }
        
        public void Start()
        {
            TwitchAPI.Settings.Validators.SkipClientIdValidation = true;
            TwitchAPI.Settings.ClientId = this.settings.ClientId;
            TwitchAPI.Settings.AccessToken = this.settings.ClientSecret;

            rewardEvery5MinutesTask = Task.Factory.StartNew(() => {
                while(true){
                    AddPointsToChatters();

                    //once every 5 minutes
                    Thread.Sleep(300000);
                }
            });
        }

        public async void AddPointsToChatters(){
            var channelOnline = await TwitchAPI.Streams.v5.BroadcasterOnline(this.settings.ChannelId);
            if (channelOnline){
                var chattersList = await TwitchAPI.Undocumented.GetChatters(this.settings.Channel);
                foreach(var user in chattersList){
                    Console.WriteLine($"Rewarding: {user.Username} - {DateTime.Now.ToShortTimeString()}");
                }

                databaseContext.RewardUsersForWatchingStream(chattersList.Select(c => c.Username));
            }
        }

        public void Restart()
        {
            throw new NotImplementedException();
        }

        public void End()
        {
            throw new NotImplementedException();
        }
    }
}