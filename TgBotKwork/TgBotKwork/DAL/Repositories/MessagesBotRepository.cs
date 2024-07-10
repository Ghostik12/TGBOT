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

        public IEnumerable<MessagesBotEntity> FindAll()
        {
            return Query<MessagesBotEntity>(@"select * from MessagesBot");
        }

        public MessagesBotEntity FindByChatId(long chatId)
        {
            return QueryFirstOrDefault<MessagesBotEntity>(@"select * from MessagesBot where chatId = :chatId_p", new { chatId_p = chatId });
        }

        public int Update(MessagesBotEntity botEntity)
        {
            return Execute(@"update MessagesBot set messagesCount = messagesCount + 1, lettersCount = lettersCount + :lettersCount where chatId = :chatId", botEntity);
        }

        public int DeleteById(long chatId)
        {
            return Execute(@"delete from users where ChatId = :chatId_p", new { chatId_p = chatId });
        }

        public interface IMessagesBotRepository
        {
            int Create(MessagesBotEntity botEntity);
            MessagesBotEntity FindByChatId(long chatId);
            IEnumerable<MessagesBotEntity> FindAll();
            int Update(MessagesBotEntity botEntity);
            int DeleteById(long chatId);
        }
    }
}
