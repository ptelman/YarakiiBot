using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using YarakiiBot.Base;

namespace YarakiiBot.IRC{
    public class IrcManager : IIrcManager
    {
        private IList<IMessageReceiver> receivers;
        private Settings settings;
        private TcpClient tcpClient;
        private StreamReader inputStream;
        private StreamWriter outputStream;
        private ILogger logger;

        public IrcManager(IOptions<Settings> settings, ILogger logger)
        {
            receivers = new List<IMessageReceiver>();
            this.settings = settings.Value;
            this.logger = logger;
            this.ConnectAsync();
        }

        public void SendMessage(string message)
        {
            try
            {
                string messageToSend = "PRIVMSG #" + this.settings.Channel + " :" + message + "\r\n";
                outputStream.WriteLine(messageToSend);
                logger.LogMessage(messageToSend);
            }
            catch (Exception e)
            {
                logger.LogException(e);
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

            Task.Factory.StartNew(() => {
                this.RecieveMessages();
            });

            outputStream.WriteLine("PASS " + this.settings.Password + "\r\n");
            outputStream.WriteLine("NICK " + this.settings.Username + "\r\n");
            outputStream.Flush();

            this.JoinChannel();
        }

        private void RecieveMessages()
        {
            string rawMessage;
            while(true)
            {
                rawMessage = inputStream.ReadLine();
                logger.LogMessage(rawMessage);

                if (rawMessage.ToLower().StartsWith("ping")){
                    outputStream.WriteLine("PONG tmi.twitch.tv\r\n");
                    outputStream.Flush();
                    continue;
                }
                if(rawMessage.Contains(":tmi.twitch.tv ")){
                    continue;
                }
                string[] splittedMessage = rawMessage.Split(':');

                if (splittedMessage.Length >= 3){
                    string user = splittedMessage[1].Split('!')[0];
                    string actualMessage = String.Join(":", splittedMessage.Skip(2).Take(splittedMessage.Length - 2));

                    logger.LogMessage($"{user} == {actualMessage}");

                    foreach(var subscriber in this.receivers){
                        subscriber.HandleIncommingMessage(user, actualMessage);
                    }
                }
            }
        }

        private void JoinChannel()
        {
            outputStream.WriteLine("JOIN #" + this.settings.Channel + "\r\n");
            outputStream.Flush();
        }
    }
}