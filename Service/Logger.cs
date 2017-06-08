
using System;
using System.IO;
using YarakiiBot.Base;

namespace YarakiiBot.Service{
    public class Logger : ILogger, IDisposable
    {
        bool initialized = false;
        FileStream logFile;
        StreamWriter logWriter;

        public void Dispose()
        {
            logWriter.Flush();
            logWriter.Dispose();
            logFile.Flush();
            logFile.Dispose();
        }

        public void Init()
        {
            if (!initialized)
            {
                logFile = System.IO.File.Create($"{DateTime.Now.ToString("dd-MM-yy_H-m")}.txt");
                logWriter = new System.IO.StreamWriter(logFile);
                initialized = true;
            }
        }

        public void LogException(Exception e)
        {
            Init();

            logWriter.WriteLine(e.ToString());
            logWriter.Flush();
        }

        public void LogMessage(string message)
        {
            Init();

            logWriter.WriteLine($"{DateTime.Now.ToString("dd-MM-yy_H-mm")} -- {message}");
            logWriter.Flush();
        }
    }
}