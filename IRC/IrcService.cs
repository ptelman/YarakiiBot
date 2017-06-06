using System;
using System.Collections.Generic;
using YarakiiBot.Base;

namespace YarakiiBot.IRC{
    public class IrcService : IIrcService
    {
        private IList<IMessageReceiver> receivers;

        public IrcService()
        {
            receivers = new List<IMessageReceiver>();
        }

        public void SendMessage(string message)
        {
            throw new NotImplementedException();
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

        private void Connect(){
            
        }
    }
}