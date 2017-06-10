using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using TwitchLib;
using TwitchLib.Models.API;
using YarakiiBot.Base;

namespace YarakiiBot.Modules{
    public class RewardActiveUsersModule : IModule
    {
        private Settings settings;

        public RewardActiveUsersModule(IOptions<Settings> settings)
        {
            this.settings = settings.Value;
        }
        
        public void Start()
        {
            TwitchAPI.Settings.ClientId = this.settings.ClientId;
            TwitchAPI.Settings.AccessToken = this.settings.ClientSecret;

            var channelRequest = TwitchAPI.Streams.v5.BroadcasterOnline(this.settings.ChannelId);
            channelRequest.Wait();
            var isOnline = channelRequest.Result;
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