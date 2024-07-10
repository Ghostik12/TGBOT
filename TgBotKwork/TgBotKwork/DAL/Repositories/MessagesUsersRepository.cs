﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TgBotKwork.DAL.Entity;
using static TgBotKwork.DAL.Repositories.MessagesUsersRepository;

namespace TgBotKwork.DAL.Repositories
{
    public class MessagesUsersRepository : TgContext, IMessagesUsersRepository
    {
        public int Create(MessagesUsersEntity messagesUsersEntity)
        {
            return Execute(@"insert into MessagesUsers (chatId, messagesCount, lettersCount) values (:chatId, :messagesCount, :lettersCount)", messagesUsersEntity);
        }

        public IEnumerable<MessagesUsersEntity> FindAll()
        {
            return Query<MessagesUsersEntity>(@"select * from MessagesUsers");
        }

        public MessagesUsersEntity FindByChatId(long chatId)
        {
            return QueryFirstOrDefault<MessagesUsersEntity>(@"select * from MessagesUsers where chatId = :chatId_p", new { chatId_p = chatId });
        }

        public int Update(MessagesUsersEntity messagesUsersEntity)
        {
            if (messagesUsersEntity.messagesCount == 1)
            return Execute(@"update MessagesUsers set messagesCount = messagesCount + 1, lettersCount = lettersCount + :lettersCount where chatId = :chatId", messagesUsersEntity);
            else 
            return Execute(@"update MessagesUsers set messagesCount = messagesCount + 0, lettersCount = lettersCount + :lettersCount where chatId = :chatId", messagesUsersEntity);
        }

        public int DeleteById(long chatId)
        {
            return Execute(@"delete from MessagesUsers where ChatId = :chatId_p", new { chatId_p = chatId });
        }

        public interface IMessagesUsersRepository
        {
            int Create(MessagesUsersEntity userEntity);
            MessagesUsersEntity FindByChatId(long chatId);
            IEnumerable<MessagesUsersEntity> FindAll();
            int Update(MessagesUsersEntity userEntity);
            int DeleteById(long chatId);
        }
    }
}
