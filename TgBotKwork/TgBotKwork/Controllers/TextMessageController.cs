using Newtonsoft.Json.Linq;
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

        public TextMessageController(ITelegramBotClient telegramBotClient)
        {
            _telegramBotClient = telegramBotClient;
        }

        public async Task Handle(Message message, CancellationToken cancellationToken)
        {
            Dictionary<string, string> par = new Dictionary<string, string>()
                {
                {"conversation_id", "123" },
                { "bot_id", "7388135449996427269" },
                {"user", "123333333" },
                {"query", $"{message.Text}" },
                {"stream", "false" }
                };
            try
            {
                switch(message.Text)
                {
                    case "/start":
                        await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"{message.Chat.Username}, приветствую тебя в этом боте");
                        break;
                    case "/admin":
                        if(message.Chat.Id == 1451999567)
                        {
                            var buttons = new List<InlineKeyboardButton[]>();
                            buttons.Add(new[]
                            {
                                InlineKeyboardButton.WithCallbackData($"Общее количество сообщений боту", $"SM"),
                                InlineKeyboardButton.WithCallbackData($"Общее количество символов боту", $"SL")
                            });
                            var keyboard = new InlineKeyboardMarkup(new[]
                            {
                                InlineKeyboardButton.WithCallbackData("hi", "SM")
                            });

                            await _telegramBotClient.SendTextMessageAsync(message.Chat.Id,"Выбери что хочешь увидеть",cancellationToken: cancellationToken,parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: keyboard);
                            
                        }
                        break;
                    default:
                        string message1 = message.Text.Replace(" ", "");
                        var countLetter = message1.Length;

                        if (!CountMessage.ContainsKey(message.Chat.Id))
                        {
                            if (countLetter < 500)
                            {
                                CountMessage.Add(message.Chat.Id, +1);
                                CountLetters.Add(message.Chat.Id, countLetter);
                                var answer = GetRequest("https://api.coze.com/open_api/v2/chat", par).Result;
                                var json = answer.Content.ReadAsStringAsync().Result;
                                var jsonObj = JObject.Parse(json);
                                var messages = (JArray)jsonObj["messages"];
                                var answerMessage = messages.FirstOrDefault(message => (string)message["type"] == "answer");

                                if (answerMessage != null)
                                {
                                    var content = (string)answerMessage["content"];
                                    await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, content);
                                    Thread.Sleep(30000);
                                }
                                foreach (var cou in CountMessage)
                                    Console.WriteLine($"{cou.Key} : {cou.Value}");
                                break;
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
                                    if (letter.Value < 500)
                                    {
                                        CountMessage[message.Chat.Id] += 1;
                                        CountLetters[message.Chat.Id] += countLetter;
                                        var answer = GetRequest("https://api.coze.com/open_api/v2/chat", par).Result;
                                        var json = answer.Content.ReadAsStringAsync().Result;
                                        var jsonObj = JObject.Parse(json);
                                        var messages = (JArray)jsonObj["messages"];
                                        var answerMessage = messages.FirstOrDefault(message => (string)message["type"] == "answer");

                                        if (answerMessage != null)
                                        {
                                            var content = (string)answerMessage["content"];
                                            await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, content);
                                            Thread.Sleep(30000);
                                        }
                                        foreach (var cou in CountMessage)
                                            Console.WriteLine($"{cou.Key} : {cou.Value}");
                                        break;
                                    }
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
                client.DefaultRequestHeaders.Add("Authorization", "Bearer pat_drM5ibXXw5tjbZ4060MQPRDDyfrnVo3BBOzy7wVoEkF8DEOWMJ1CgiAovx5x9La5");
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

        public async Task HandleCallBack(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
                return;

            string languageText = callbackQuery.Data;
            switch (languageText)
            {
                case "SM":
                    var sm = CountMessage.Values.Sum();
                    await _telegramBotClient?.SendTextMessageAsync(callbackQuery.From.Id, $"{sm}", cancellationToken: ct);
                    break;
                case "SL":
                    var sl = CountLetters.Values.Sum();
                    await _telegramBotClient?.SendTextMessageAsync(callbackQuery.From.Id, $"{sl}", cancellationToken: ct);
                    break;
            }
        }
    }
}