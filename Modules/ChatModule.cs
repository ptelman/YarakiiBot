using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using TwitchLib;
using TwitchLib.Events.Client;
using TwitchLib.Models.Client;
using YarakiiBot.Base;

namespace YarakiiBot.Modules{
    public class ChatModule : IModule
    {
        private Settings settings;
        private TwitchClient client;

        private List<IMessageReceiver> allMessageReceivers = new List<IMessageReceiver>();
        private List<ICommandReceiver> commandReceivers = new List<ICommandReceiver>();

        public ChatModule(IOptions<Settings> settings)
        {
            this.settings = settings.Value;
        }
        
        public void Start()
        {
            var credentials = new ConnectionCredentials(this.settings.Username, this.settings.AccessToken);
            client = new TwitchClient(credentials, this.settings.Channel, '!', '!', false);
            client.OnConnected+= Client_OnConnected;
            client.OnMessageReceived+= Client_OnGotMessage;
            client.OnJoinedChannel+= Client_OnJoinedChannel;
            client.Connect();
        }

        public void SubscribeToNewMessages(IMessageReceiver messageReceiver)
        {
            allMessageReceivers.Add(messageReceiver);
        }

        public void SubscribeToCommands(ICommandReceiver commandReceiver)
        {
            commandReceivers.Add(commandReceiver);
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            Console.WriteLine($"Joined Channel: {e.Channel}");
        }

        private void Client_OnGotMessage(object sender, OnMessageReceivedArgs e)
        {
            foreach (var handler in allMessageReceivers){
                handler.HandleIncommingMessage(e.ChatMessage.Username, e.ChatMessage.Message);
            }
            if (e.ChatMessage.Message.StartsWith('!')){
                foreach (var commandHandler in commandReceivers){
                    string response = commandHandler.HandleCommand(e.ChatMessage.Username, e.ChatMessage.Message);
                    if (!String.IsNullOrWhiteSpace(response)){
                        client.SendMessage(response);
                    }
                }
            }

            Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}] {e.ChatMessage.Username}:    {e.ChatMessage.Message}");
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine("Client has Connected");
        }

        private void onCommandReceived(object sender, OnWhisperCommandReceivedArgs e) {
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