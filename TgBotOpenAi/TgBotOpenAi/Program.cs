using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TgBotOpenAi
{
    internal class Program
    {
        private static ITelegramBotClient _client;
        private static ReceiverOptions _receiverOptions;

        static async Task Main(string[] args)
        {
            _client = new TelegramBotClient("6701661991:AAF8KBsKh04bMicis2u2WHNJ_eJ1cVPSvrM");

            _receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[]
                {
                    UpdateType.Message,
                },
                ThrowPendingUpdates = true,
            };

            using var cts = new CancellationTokenSource();

            _client.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token);

            var me = await _client.GetMeAsync();
            Console.WriteLine($"{me.FirstName} launch");

            await Task.Delay(-1);
        }

        private static async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
        {
            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        {
                            Console.WriteLine("Wirten");
                            return;
                        }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            // Тут создадим переменную, в которую поместим код ошибки и её сообщение 
            var ErrorMessage = error switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => error.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
