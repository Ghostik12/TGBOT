using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgBotKwork.BLL.Models
{
    public class User
    {
        public long ChatId { get; set; }
        public long MessagesCount { get; set; }
        public long LettersCount { get; set; }
    }
}
