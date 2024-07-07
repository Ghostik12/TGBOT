using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TgBotOpenAi
{
     class Program
    {
        private static ITelegramBotClient _client;
        private static ReceiverOptions _receiverOptions;
        private static HttpClient _httpClient;
        private static Dictionary<long, long> Count = new Dictionary<long, long>();

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

        public class CustomConverter
        {
            [JsonProperty("role")]
            public string? Role { get; set; }
            [JsonProperty("type")]
            public string? Type { get; set; }
            [JsonProperty("content")]
            public string? Content { get; set; }
            [JsonProperty("content_type")]
            public string? ContentType { get; set; }
        }

        private static async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
        {
            try
            { 
                if (update.Type == UpdateType.Message)
                {
                    string message = Convert.ToString(update.Message).Replace(" ","");
                    var countWords = message.Length;
                    if (!Count.ContainsKey(update.Message.Chat.Id))
                    {
                        Count.Add(update.Message.Chat.Id, + 1);
                    }
                    else
                    {
                        Count[update.Message.Chat.Id] += 1;
                    }

                    Dictionary<string, string> par = new Dictionary<string, string>()
                    {
                        {"conversation_id", "123" },
                        {"bot_id", "7388135449996427269" },
                        {"user", "123333333" },
                        {"query", $"{update.Message}" },
                        {"stream", "false" }
                    };


                    var answer = GetRequest("https://api.coze.com/open_api/v2/chat", par).Result;
                    var json = answer.Content.ReadAsStringAsync().Result;
                    var jsonObj = JObject.Parse(json);
                    var messages = (JArray)jsonObj["messages"];
                    var answerMessage = messages.FirstOrDefault(message => (string)message["type"] == "answer");

                    if (answerMessage != null)
                    {
                        var content = (string)answerMessage["content"];
                        await client.SendTextMessageAsync(update.Message.Chat.Id, content);
                    }

                    foreach (var cou in Count)
                        Console.WriteLine($"{cou.Key} : {cou.Value}");
                }
                else if(update.Message.Document != null)
                {
                    await _client.SendTextMessageAsync(update.Message.Chat.Id, "Извините, но документ не принимается");
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
    }
}
