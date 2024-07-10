using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TgBotKwork.BLL.Models
{
    public class MessagesBot
    {
        public long ChatId { get; set; }
        public long MessagesCount { get; set; }
        public long LettersCount { get; set; }

        public MessagesBot(long id, long messagesCount, long lettersCoutn)
        {
            ChatId = id;
            MessagesCount = messagesCount;
            LettersCount = lettersCoutn;
        }
    }
}
