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
    internal class Program
    {
        private static ITelegramBotClient _client;
        private static ReceiverOptions _receiverOptions;
        private static HttpClient _httpClient;

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

                if (update.Message == update.Message)
                {
                    //var request = new HttpRequestMessage(HttpMethod.Post, "https://api.coze.com/open_api/v2/chat");

                    //request.Headers.Add("Accept", "application/json");
                    //request.Headers.Add("Host", "api.coze.com");
                    //request.Headers.Add("Connection", "keep-alive");
                    //request.Headers.Add("Authorization", "Bearer pat_3OvmHRzUnmA4ny4FXdfvyXLSFgDBKRVZNtZEOCwHbkfKzGZsVjggr8dJvUxN7I18");

                    //var con = "{\r\n    \"conversation_id\": \"123\",\r\n    \"bot_id\": \"7387461427646939141\",\r\n    \"user\": \"123333333\",\r\n    \"query\": \"Hi\",\r\n    \"stream\":false\r\n}";
                    //request.Content = new StringContent(con, Encoding.UTF8, "application/json");
                    //var response = await _httpClient.SendAsync(request);
                    //if (response.StatusCode != HttpStatusCode.OK)
                    //{
                    //    string str = await response.Content.ReadAsStringAsync();
                    //    throw new Exception("Response is " + str + "\r\n" + "Code is " + response.StatusCode);
                    //}

                    //var responseBody = await response.Content.ReadAsStringAsync();
                    //var resp = responseBody.ToString();
                    //await _client.SendTextMessageAsync(update.Id, resp);

                    Dictionary<string, string> par = new Dictionary<string, string>()
                    {
                        //{"Authorization", "Bearer pat_3OvmHRzUnmA4ny4FXdfvyXLSFgDBKRVZNtZEOCwHbkfKzGZsVjggr8dJvUxN7I18" },
                        //{"Content-Type", "application/json" },
                        //{"Accept", "*/*" },
                        //{"Host", "api.coze.com" },
                        //{"Connection", "keep-alive" },
                        {"conversation_id", "123" },
                        {"bot_id", "7388135449996427269" },
                        {"user", "123333333" },
                        {"query", $"{update.Message}" },
                        {"stream", "false" }
                    };

                    var answer = GetRequest("https://api.coze.com/open_api/v2/chat", par).Result;
                   var json = answer.Content.ReadAsStringAsync().Result;
                    CustomConverter? a = JsonConvert.DeserializeObject<CustomConverter>(json);
                    string b = JsonConvert.SerializeObject(a, Formatting.Indented);
                    //var yourObject = System.Text.Json.JsonDocument.Parse(json);
                    var jsonObj = JObject.Parse(json);
                    var messages = (JArray)jsonObj["messages"];
                    var answerMessage = messages.FirstOrDefault(message => (string)message["type"] == "answer");

                    if (answerMessage != null)
                    {
                        var content = (string)answerMessage["content"];
                        Console.WriteLine(content);
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
