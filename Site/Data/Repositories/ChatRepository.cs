using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Entities;
using site.Helpers;
using Site.Data;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace site.Repositories
{
    public class ChatRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentDate _currentDate;

        public ChatRepository(ApplicationDbContext context, ICurrentDate currentDate)
        {
            _context = context;
            _currentDate = currentDate;
        }

        public async Task CreateChat(MessageContext context)
        {
            var chat = _context.Chats.FirstOrDefault(e => 
                    (e.InitiatorUserId == context.UserId && e.RecipientUserId == context.RecipientUserId) ||
                    (e.InitiatorUserId == context.RecipientUserId && e.RecipientUserId == context.UserId)
                    );

            if (chat == null)
            {
                chat = CreateNewChat(context);
                _context.Chats.Add(chat);
            }

            var message = new ChatMessage {
                Chat = chat,
                CreatedOn = _currentDate.Now(),
                Message = context.Message,
                SiteUserId = context.MessageUserId                
            };

            _context.ChatMessages.Add(message);

            await _context.SaveChangesAsync();
        }

        private Chat CreateNewChat(MessageContext context)
        {
            return new Chat
            {
                CreatedOn = _currentDate.Now(),
                Subject = "Chat between 2 people",
                InitiatorUserId = context.UserId,
                RecipientUserId = context.RecipientUserId,
                LastModifiedOn = _currentDate.Now()              
            };
        }

        public async Task<IEnumerable<Chat>> GetChatsByUser(int userId)
        {
            return await _context.Chats.AsNoTracking()
                                 .Where(e => e.InitiatorUserId == userId || e.RecipientUserId == userId)
                                 .ToListAsync();
        }

        public async Task<Chat> GetChat(int chatId)
        {
            return await _context.Chats.FirstOrDefaultAsync(e => e.Id == chatId);
        }

        public async Task<List<ChatMessage>> GetChatMessages(int chatId, int userId)
        {
            return await (from message in _context.ChatMessages
                          join chat in _context.Chats on message.ChatId equals chat.Id
                          join user in _context.SiteUsers on message.SiteUserId equals user.Id
                          where message.ChatId == chatId && (chat.InitiatorUserId == userId || chat.RecipientUserId == userId)
                          select new ChatMessage
                          {
                              Chat = chat,
                              SiteUser = user,
                              CreatedOn = message.CreatedOn,
                              Id = chat.Id,
                              Message = message.Message,
                              ProfilePicture = user.ProfilePicturePath,
                              ChatId = message.ChatId,
                              SiteUserId = message.SiteUserId
                          }).ToListAsync();
        }

        public async Task<List<ChatMessage>> GetRecentMessages(int userId)
        {
            return await (from message in _context.ChatMessages
                   join chat in _context.Chats on message.ChatId equals chat.Id
                   join user in _context.SiteUsers on message.SiteUserId equals user.Id
                   where chat.InitiatorUserId == userId || chat.RecipientUserId == userId
                   orderby message.CreatedOn descending
                   select new ChatMessage
                   {
                       Chat = chat,
                       SiteUser = user,
                       CreatedOn = message.CreatedOn,
                       Id = chat.Id,
                       Message = message.Message,
                       ProfilePicture = user.ProfilePicturePath,
                       ChatId = message.ChatId,
                       SiteUserId = message.SiteUserId
                   }).ToListAsync();
        }
    }
}
