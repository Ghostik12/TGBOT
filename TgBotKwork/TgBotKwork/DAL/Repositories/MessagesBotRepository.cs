using Telegram.Bot.Types;
using TgBotKwork.DAL.Entity;
using static TgBotKwork.DAL.Repositories.MessagesBotRepository;

namespace TgBotKwork.DAL.Repositories
{
    internal class MessagesBotRepository : TgContext, IMessagesBotRepository
    {
        public int Create(MessagesBotEntity botEntity)
        {
            return Execute(@"insert into MessagesBot (chatId, messagesCount, lettersCount) values (:chatId, :messagesCount, :lettersCount)", botEntity);
        }

        public long FindAll(long chatId)
        {
            if (chatId == 0)
               return QueryFirstOrDefault<long>(@"select SUM(messagesCount) from MessagesBot");

            else
                return QueryFirstOrDefault<long>(@"select SUM(lettersCount) from MessagesBot");
        }

        public MessagesBotEntity FindByChatId(long chatId)
        {
            return QueryFirstOrDefault<MessagesBotEntity>(@"select * from MessagesBot where chatId = :chatId_p", new { chatId_p = chatId });
        }

        public int Update(MessagesBotEntity botEntity)
        {
            if(botEntity.messagesCount == 1)
                return Execute(@"update MessagesBot set messagesCount = messagesCount + 1, lettersCount = lettersCount + :lettersCount where chatId = :chatId", botEntity);
            else
                return Execute(@"update MessagesBot set messagesCount = messagesCount + 0, lettersCount = lettersCount + :lettersCount where chatId = :chatId", botEntity);
        }

        public int UpdateStatistic()
        {
               return Execute(@"update MessagesBot set messagesCount = 0,lettersCount = 0 where messagesCount is not 0");
        }

        public interface IMessagesBotRepository
        {
            int Create(MessagesBotEntity botEntity);
            MessagesBotEntity FindByChatId(long chatId);
            long FindAll(long chatId);
            int Update(MessagesBotEntity botEntity);
            int UpdateStatistic();
        }
    }
}
