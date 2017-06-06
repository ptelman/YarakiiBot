using System;
using System.IO;
using YarakiiBot.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using YarakiiBot.Base;
using YarakiiBot.IRC;

namespace YarakiiBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var serviceCollection = new ServiceCollection();
            ConfigureSettings(serviceCollection);
            ConfigureSingletons(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var test = serviceProvider.GetService<IIrcService>();
        }

        private static void ConfigureSingletons(IServiceCollection serviceCollection){
            serviceCollection.AddTransient<IIrcService, IrcService>();
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
