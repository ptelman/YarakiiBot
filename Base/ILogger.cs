using System;

namespace YarakiiBot.Base{
    public interface ILogger
    {
        void LogException(Exception e);
        void LogMessage(string message);
    }
}