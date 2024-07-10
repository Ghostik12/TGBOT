using static TgBotKwork.DAL.Repositories.MessagesBotRepository;
using TgBotKwork.DAL.Entity;
using TgBotKwork.BLL.Models;
using TgBotKwork.DAL.Repositories;

namespace TgBotKwork.BLL.Services
{
    public class MessagesBotService
    {
        IMessagesBotRepository messagesBotRepository;

        public MessagesBotService()
        {
            messagesBotRepository = new MessagesBotRepository();
        }

        public void Add(MessagesBot message)
        {
            var messageBotEntity = new MessagesBotEntity()
            {
                chatId = message.ChatId,
                messagesCount = message.MessagesCount,
                lettersCount = message.LettersCount,
            };

            messagesBotRepository.Create(messageBotEntity);

            if (messagesBotRepository.Create(messageBotEntity) == 0)
                throw new Exception();
        }

        public void Update(Bots message) 
        {
            var messageBotEntity = new MessagesBotEntity()
            {
                chatId = message.ChatId,
                messagesCount = message.MessagesCount,
                lettersCount = message.LettersCount,
            };
        }
        public MessagesBot GetInfo(long chatId) 
        {
            var findGetInfo = messagesBotRepository.FindByChatId(chatId);
            if (findGetInfo is null) throw new Exception();

            return ConstructUserModel(findGetInfo);
        }

        private MessagesBot ConstructUserModel(MessagesBotEntity messagesBotEntity)
        {

            return new MessagesBot(messagesBotEntity.chatId,
                messagesBotEntity.messagesCount,
                messagesBotEntity.lettersCount);
        }
    }
}
