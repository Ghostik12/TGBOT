using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Module11.Controllers
{
    //public class InlineKeyboardController
    //{
    //    //private readonly ITelegramBotClient _telegramBotClient;

    //    //public InlineKeyboardController(ITelegramBotClient telegramBotClient)
    //    //{
    //    //    _telegramBotClient = telegramBotClient;
    //    //}

    //    //public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
    //    //{
    //    //    if (callbackQuery?.Data == null)
    //    //        return;

    //    //    string languageText = callbackQuery.Data;
    //    //    switch (languageText)
    //    //    {
    //    //        case "SM":
    //    //            break;
    //    //        case "SL":
    //    //            break;
    //    //    }

    //    //    await _telegramBotClient.SendTextMessageAsync(callbackQuery.From.Id,
    //    //        $"<b>Язык аудио - {languageText}.{Environment.NewLine}</b>" + $"{Environment.NewLine}Можно поменять в главном меню.", cancellationToken: ct, parseMode: ParseMode.Html);
    //    //    Console.WriteLine($"Контроллер {GetType().Name} получил сообщение");
    //    //    //await _telegramBotClient.SendTextMessageAsync(callbackQuery.From.Id, $"Обнаружено нажатие на кнопку {callbackQuery.Data}", cancellationToken: ct);
    //    //}
    //}
}