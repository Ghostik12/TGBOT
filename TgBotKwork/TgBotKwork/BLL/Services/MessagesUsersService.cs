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
             if (messagesUsersRepository.FindByChatId(messagesUsers.ChatId) == null)
                 throw new ArgumentNullException();

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
            //if (findGetInfo is null) throw new Exception();

            return ConstructUserModel(findGetInfo);
        }
        private MessagesUsers? ConstructUserModel(MessagesUsersEntity? messagesUsersEntity)
        {
            return new MessagesUsers(messagesUsersEntity.chatId,
                messagesUsersEntity.messagesCount,
                messagesUsersEntity.lettersCount);
        }
    }
}
