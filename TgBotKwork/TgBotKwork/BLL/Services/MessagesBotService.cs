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

        public void Add(Bots message)
        {
            var messageBotEntity = new MessagesBotEntity()
            {
                chatId = message.ChatId,
                messagesCount = message.MessagesCount,
                lettersCount = message.LettersCount,
            };

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
            messagesBotRepository.Update(messageBotEntity);
        }
        public MessagesBot GetInfo(long chatId) 
        {
            var findGetInfo = messagesBotRepository.FindByChatId(chatId);
            if (findGetInfo == null)
                return ConstructModelNull();

            return ConstructUserModel(findGetInfo);
        }

        public int UpdateStatistic()
        {
            return messagesBotRepository.UpdateStatistic();
        }

        public long GetSum(long chatId)
        {
            return messagesBotRepository.FindAll(chatId);
        }

        private MessagesBot ConstructUserModel(MessagesBotEntity messagesBotEntity)
        {

            return new MessagesBot(messagesBotEntity.chatId,
                messagesBotEntity.messagesCount,
                messagesBotEntity.lettersCount);
        }

        private MessagesBot ConstructModelNull()
        {
            return new MessagesBot(0, 0, 0);
        }
    }
}
