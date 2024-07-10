using System;
using System.Collections.Generic;
using System.Linq;


namespace TgBotKwork.BLL.Models
{
    public class MessagesUsers
    {
        public long ChatId { get; set; }
        public long MessagesCount { get; set; }
        public long LettersCount { get; set; }

        public MessagesUsers(long id, long messagesCount, long lettersCoutn)
        {
            ChatId = id;
            MessagesCount = messagesCount;
            LettersCount = lettersCoutn;
        }
    }
}
