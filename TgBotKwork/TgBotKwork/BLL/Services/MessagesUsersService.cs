using TgBotKwork.BLL.Models;
using TgBotKwork.DAL.Repositories;
using static TgBotKwork.DAL.Repositories.MessagesUsersRepository;
using TgBotKwork.DAL.Entity;

namespace TgBotKwork.BLL.Services
{
    public class MessagesUsersService
    {
        IMessagesUsersRepository messagesUsersRepository;

        public MessagesUsersService() 
        {
            messagesUsersRepository = new MessagesUsersRepository();
        }
        public void Add(User messagesUsers)
        {
             

             var messagesUser = new MessagesUsersEntity()
             {
             chatId = messagesUsers.ChatId,
             messagesCount = messagesUsers.MessagesCount,
             lettersCount = messagesUsers.LettersCount,
             };

            messagesUsersRepository.Create(messagesUser);

            if (messagesUsersRepository.Create(messagesUser) == 0)
                throw new Exception();
        }

        public void Update(User messagesUsers) 
        {
            var messagesUser = new MessagesUsersEntity()
            {
                chatId = messagesUsers.ChatId,
                messagesCount = messagesUsers.MessagesCount,
                lettersCount = messagesUsers.LettersCount,
            };

            messagesUsersRepository.Update(messagesUser);
        }

        public MessagesUsers? GetInfo(long chatId) 
        {
            var findGetInfo = messagesUsersRepository.FindByChatId(chatId);
            if (findGetInfo is null)
                return ConstructModelNull();

            return ConstructUserModel(findGetInfo);
        }

        public int UpdateStatistic()
        {
            return messagesUsersRepository.UpdateStatistic();
        }

        public long GetSum(long chatId)
        {
            return messagesUsersRepository.FindAll(chatId);
        }

        private MessagesUsers? ConstructUserModel(MessagesUsersEntity? messagesUsersEntity)
        {
            return new MessagesUsers(messagesUsersEntity.chatId,
                messagesUsersEntity.messagesCount,
                messagesUsersEntity.lettersCount);
        }

        private MessagesUsers ConstructModelNull()
        {
            return new MessagesUsers(0, 0, 0);
        }
    }
}
