using Newtonsoft.Json.Linq;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Module11.Controllers
{
    public class TextMessageController
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private static ReceiverOptions _receiverOptions;
        private static HttpClient _httpClient;
        private static Dictionary<long, long> CountMessage = new Dictionary<long, long>();
        private static Dictionary<long, long> CountLetters = new Dictionary<long, long>();
        private static Dictionary<long, long> CountMessageBot = new Dictionary<long, long>();
        private static Dictionary<long, long> CountLettersBot = new Dictionary<long, long>();
        private static List<long> Admins = new List<long>();
        private static TimeSpan sendTrothle = TimeSpan.FromSeconds(30);
        private static DateTime lastTimeMsg = new DateTime();

        public TextMessageController(ITelegramBotClient telegramBotClient)
        {
            _telegramBotClient = telegramBotClient;
        }

        public async Task Handle(Message message, CancellationToken cancellationToken)
        {
            Dictionary<string, string> par = new Dictionary<string, string>()
                {
                {"conversation_id", "123" },
                { "bot_id", "7375535948957745158" },
                {"user", "123333333" },
                {"query", $"{message.Text}" },
                {"stream", "false" }
                };
            try
            {
                switch (message.Text)
                {
                    case "/start":
                        await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"{message.Chat.Username}, приветствую тебя в нашем боте!\n" +
                            $"Внимание!\nСообщение можно отправлять 1 раз в 30 секунд, а также у есть лимит на количеству символов их: 500 от вас и 3000 от бота.");
                        break;
                    case "/admin":
                        if (message.Chat.Id == 1451999567 || message.Chat.Id == 312720548)
                        {
                            var buttons = new InlineKeyboardMarkup(new[]
                            {
                            new[]{
                                InlineKeyboardButton.WithCallbackData($"Сумма ✉ боту", $"SMU"),
                                InlineKeyboardButton.WithCallbackData($"Сумма символов боту", $"SLU"),
                            },
                            new[]{
                                InlineKeyboardButton.WithCallbackData($"Сумма ✉ бота", $"SMB"),
                                InlineKeyboardButton.WithCallbackData($"Сумма символов бота", $"SLB")
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData($"Обновление лимитов", $"Upd")
                            }
                            });
                            await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, "Выбери что хочешь увидеть", cancellationToken: cancellationToken, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: buttons);


                        }
                        break;
                    case "/information.start":

                        break;
                    default:
                        Console.WriteLine($"{message.Chat.Id}, write to bot");
                        var timeNow = DateTime.Now;
                        var countLetter = message.Text.Length;

                        if (!CountMessage.ContainsKey(message.Chat.Id))
                        {
                            if (countLetter < 500)
                            {
                                CountMessage.Add(message.Chat.Id, + 1);
                                CountLetters.Add(message.Chat.Id, + countLetter);
                                var answer = GetRequest("https://api.coze.com/open_api/v2/chat", par).Result;
                                var json = answer.Content.ReadAsStringAsync().Result;
                                var jsonObj = JObject.Parse(json);
                                var messages = (JArray)jsonObj["messages"];
                                var answerMessage = messages.FirstOrDefault(message => (string)message["type"] == "answer");
                                if (answerMessage != null)
                                {
                                    var content = (string)answerMessage["content"];
                                    var letters = content.Length;
                                    if (letters > 3000)
                                    {
                                        await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, "Боту позволенно отправлять не более 3000 символов за раз!");
                                        return;
                                    }
                                    await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, content);
                                    CountLettersBot.Add(message.Chat.Id, + letters);
                                    CountMessageBot.Add(message.Chat.Id, + 1);
                                    lastTimeMsg = DateTime.Now;


                                }
                                return;
                            }
                            await _telegramBotClient?.SendTextMessageAsync(message.Chat.Id, "Вы превысили лимит в 500 символов");
                            return;
                        }
                        else if (CountMessage.ContainsKey(message.Chat.Id))
                        {
                            foreach (var letter in CountLetters)
                            {
                                if (letter.Key == message.Chat.Id)
                                {
                                    if (timeNow - lastTimeMsg < sendTrothle)
                                    {
                                        return;
                                    }

                                    CountLetters[message.Chat.Id] += countLetter;

                                    if (message.Text.Length < 500 || CountLetters[message.Chat.Id] < 500)
                                    {
                                        CountMessage[message.Chat.Id] += 1;
                                        var answer = GetRequest("https://api.coze.com/open_api/v2/chat", par).Result;
                                        var json = answer.Content.ReadAsStringAsync().Result;
                                        var jsonObj = JObject.Parse(json);
                                        var messages = (JArray)jsonObj["messages"];
                                        var answerMessage = messages.FirstOrDefault(message => (string)message["type"] == "answer");

                                        if (answerMessage != null)
                                        {
                                            var content = (string)answerMessage["content"];
                                            var letters = content.Length;
                                            foreach (var letterSum in CountLettersBot)
                                                if (letterSum.Key == message.Chat.Id)
                                                {
                                                    CountLettersBot[message.Chat.Id] += letters;
                                                    if (letters > 3000 || CountLettersBot[message.Chat.Id] > 3000)
                                                    {
                                                        await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, "Боту позволенно отправлять не более 3000 символов в сумме!");
                                                        return;
                                                    }
                                                    await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, content);
                                                    CountMessageBot[message.Chat.Id] += 1;
                                                    lastTimeMsg = DateTime.Now;
                                                }
                                        }
                                        return;
                                    }
                                    await _telegramBotClient?.SendTextMessageAsync(message.Chat.Id, "Вы превысили лимит в 500 символов");
                                    return;
                                }
                                await _telegramBotClient?.SendTextMessageAsync(message.Chat.Id, "Вы превысили лимит в 500 символов");
                                return;
                            }
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        static async Task<HttpResponseMessage> GetRequest(string address, Dictionary<string, string> parameters)
        {
            HttpClient client = new HttpClient();
            try
            {
                Uri uri = new Uri(address);
                var content = new FormUrlEncodedContent(parameters);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer pat_rDxvy0zWMaq5VleoE9lQdl9Nsc4CJFX9SSdex2nEl190ujA5ZK8fpuNEDqKN91j3");
                client.DefaultRequestHeaders.Add("Accept", "*/*");
                client.DefaultRequestHeaders.Add("Host", "api.coze.com");
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");

                return await client.PostAsync(address, content);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.ToString()}");
            }
            finally
            {
                client.Dispose();
            }
            return null;
        }

        public async Task HandleCallBack(CallbackQuery? callbackQuery, CancellationToken ct, Update update)
        {
            var buttons = new InlineKeyboardMarkup(new[]
            {
                new[]{
                    InlineKeyboardButton.WithCallbackData($"Сумма ✉ боту", $"SMU"),
                    InlineKeyboardButton.WithCallbackData($"Сумма символов боту", $"SLU"),
                },
                new[]{
                    InlineKeyboardButton.WithCallbackData($"Сумма ✉ бота", $"SMB"),
                    InlineKeyboardButton.WithCallbackData($"Сумма символов бота", $"SLB")
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData($"Обновление лимитов", $"Upd")
                }
            });

            if (callbackQuery?.Data == null)
                return;

            string languageText = callbackQuery.Data;
            switch (languageText)
            {
                case "SMU":
                    var smu = CountMessage.Values.Sum();
                    await _telegramBotClient?.SendTextMessageAsync(callbackQuery.From.Id, $"Сумма сообщений боту: {smu}", cancellationToken: ct);
                    await _telegramBotClient.SendTextMessageAsync(callbackQuery.From.Id, "Выбери что хочешь увидеть", parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: buttons);
                    break;
                case "SLU":
                    var slu = CountLetters.Values.Sum();
                    await _telegramBotClient?.SendTextMessageAsync(callbackQuery.From.Id, $"Сумма символов боту: {slu}", cancellationToken: ct);
                    await _telegramBotClient.SendTextMessageAsync(callbackQuery.From.Id, "Выбери что хочешь увидеть", parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: buttons);
                    break;
                case "SMB":
                    var smb = CountMessageBot.Values.Sum();
                    await _telegramBotClient?.SendTextMessageAsync(callbackQuery.From.Id, $"Сумма сообщений от бота: {smb}", cancellationToken: ct);
                    await _telegramBotClient.SendTextMessageAsync(callbackQuery.From.Id, "Выбери что хочешь увидеть", parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: buttons);
                    break;
                case "SLB":
                    var slb = CountLettersBot.Values.Sum();
                    await _telegramBotClient?.SendTextMessageAsync(callbackQuery.From.Id, $"Сумма симовлов от бота: {slb}", cancellationToken: ct);
                    await _telegramBotClient.SendTextMessageAsync(callbackQuery.From.Id, "Выбери что хочешь увидеть", parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: buttons);
                    break;
                case "Upd":
                    foreach (var count in CountMessage)
                        CountMessage[count.Key] = 0;

                    foreach (var count in CountMessageBot)
                        CountMessageBot[count.Key] = 0;

                    foreach (var count in CountLetters)
                        CountLetters[count.Key] = 0;

                    foreach (var count in CountLettersBot)
                        CountLettersBot[count.Key] = 0;

                    await _telegramBotClient.SendTextMessageAsync(callbackQuery.From.Id, $"Информация обновленна");
                    await _telegramBotClient.SendTextMessageAsync(callbackQuery.From.Id, "Выбери что хочешь увидеть", parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: buttons);
                    break;
            }
        }
    }
}