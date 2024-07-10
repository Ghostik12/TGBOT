using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgBotKwork.DAL.Entity
{
    public class MessagesUsersEntity
    {
        public long chatId { get; set; }
        public long messagesCount { get; set; }
        public long lettersCount { get; set; }
    }
}
