using System;
using System.IO;
using YarakiiBot.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using YarakiiBot.Base;
using YarakiiBot.IRC;
using System.Threading;
using YarakiiBot.Service;
using YarakiiBot.MessageHandler;
using YarakiiBot.Modules.ChatModule;

namespace YarakiiBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureSettings(serviceCollection);
            ConfigureSingletons(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var chatModule = serviceProvider.GetService<ChatModule>();
            chatModule.Start();

            //var userMessagesCountHandler = serviceProvider.GetService<UserMessagesCount>();
            //ircManager.Subscribe(userMessagesCountHandler);
            Thread.Sleep(10000000);
        }

        private static void ConfigureSingletons(IServiceCollection serviceCollection){
            serviceCollection.AddTransient<IIrcManager, IrcManager>();
            serviceCollection.AddTransient<ILogger, Logger>();
            serviceCollection.AddSingleton<DatabaseContext,DatabaseContext>();
            serviceCollection.AddSingleton<UserMessagesCount,UserMessagesCount>();
            serviceCollection.AddSingleton<ChatModule,ChatModule>();
        }

        private static void ConfigureSettings(IServiceCollection serviceCollection){

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("app-settings.json", false)
                .Build();
            serviceCollection.AddOptions();
            serviceCollection.Configure<Settings>(configuration.GetSection("Settings"));
        }
    }
}
