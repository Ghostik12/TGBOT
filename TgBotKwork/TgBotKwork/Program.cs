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

            services.AddSingleton<ITelegramBotClient>(provide => new TelegramBotClient("6701661991:AAF8KBsKh04bMicis2u2WHNJ_eJ1cVPSvrM"));
            services.AddHostedService<Bot>();

            services.AddTransient<DefaultMessage>();
            //services.AddTransient<InlineKeyboardController>();
            services.AddTransient<TextMessageController>();
            services.AddTransient<VoiceMessageController>();
        }
    }
}