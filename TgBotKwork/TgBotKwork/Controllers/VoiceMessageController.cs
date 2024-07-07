using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace Module11.Controllers
{
    public class VoiceMessageController
    {
        private readonly ITelegramBotClient _telegramBotClient;
        public VoiceMessageController(ITelegramBotClient telegramBotClient)
        {
            _telegramBotClient = telegramBotClient;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            var fileId = message.Voice?.FileId;
            if (fileId == null)
                return;

            await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, "No", cancellationToken: ct);
        }
    }
}