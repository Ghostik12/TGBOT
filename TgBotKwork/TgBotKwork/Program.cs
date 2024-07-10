using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Module11;
using System;
using System.Text;
using Telegram.Bot;
using Module11.Controllers;

namespace Module11
{
    class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services))
                .UseConsoleLifetime()
                .Build();

            Console.WriteLine("Servives launch");

            await host.RunAsync();
            Console.WriteLine("Services stop");
        }

        public static void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton<ITelegramBotClient>(provide => new TelegramBotClient("7420207255:AAHU12_FVEsEipI9K3twaFOjTyPJ9z1yh14"));
            services.AddHostedService<Bot>();

            services.AddTransient<DefaultMessage>();
            services.AddTransient<TextMessageController>();
            services.AddTransient<VoiceMessageController>();
        }
    }
}