using System;
using System.IO;
using YarakiiBot.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using YarakiiBot.Base;
using System.Threading;
using YarakiiBot.Service;
using YarakiiBot.MessageHandler;
using YarakiiBot.Modules;

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

            var rewardModule = serviceProvider.GetService<RewardActiveUsersModule>();
            rewardModule.Start();


            #region CHAT MODULE
            // var chatModule = serviceProvider.GetService<ChatModule>();
            // chatModule.Start();

            // var userMessagesCountHandler = serviceProvider.GetService<UserMessagesCount>();
            // chatModule.SubscribeToNewMessages(userMessagesCountHandler);
            // chatModule.SubscribeToCommands(userMessagesCountHandler);

            //ircManager.Subscribe(userMessagesCountHandler);
            #endregion

            Thread.Sleep(10000000);
        }

        private static void ConfigureSingletons(IServiceCollection serviceCollection){
            serviceCollection.AddTransient<ILogger, Logger>();
            serviceCollection.AddSingleton<DatabaseContext,DatabaseContext>();
            serviceCollection.AddSingleton<UserMessagesCount,UserMessagesCount>();
            serviceCollection.AddSingleton<ChatModule,ChatModule>();
            serviceCollection.AddSingleton<RewardActiveUsersModule,RewardActiveUsersModule>();
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
