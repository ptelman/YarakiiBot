using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using Microsoft.Extensions.Options;
using YarakiiBot.Base;

namespace YarakiiBot.IRC{
    public class IrcService : IIrcService
    {
        private IList<IMessageReceiver> receivers;
        private Settings settings;
        private TcpClient tcpClient;
        private StreamReader inputStream;
        private StreamWriter outputStream;

        public IrcService(IOptions<Settings> settings)
        {
            receivers = new List<IMessageReceiver>();
            this.settings = settings.Value;
            this.ConnectAsync();
        }

        public void SendMessage(string message)
        {
            try
            {
                outputStream.WriteLine("PRIVMSG #" + this.settings.Channel + " :" + message + "\r\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Data);
            }
            finally
            {
                outputStream.Flush();
            }
        }

        public void Subscribe(IMessageReceiver messageReceiver)
        {
            receivers.Add(messageReceiver);
        }

        public void Unsubscribe(IMessageReceiver messageReceiver)
        {
            if (receivers.Contains(messageReceiver))
                receivers.Remove(messageReceiver);
        }

        private async void ConnectAsync()
        {
            tcpClient = new TcpClient();
            await tcpClient.ConnectAsync("irc.chat.twitch.tv", 6667);

            inputStream = new StreamReader(tcpClient.GetStream());
            outputStream = new StreamWriter(tcpClient.GetStream());

            outputStream.WriteLine("PASS " + this.settings.Password);
            outputStream.WriteLine("NICK " + this.settings.Username);
            outputStream.Flush();

            this.joinChannel();
            this.SendMessage("test");
        }

        private void joinChannel()
        {
            outputStream.WriteLine("JOIN #" + this.settings.Channel);
            outputStream.Flush();
        }
    }
}