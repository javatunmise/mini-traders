using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using site.Repositories;
using Site.Data;

namespace site.Pages.Profile
{
    public class MessagesModel : PageModel
    {
        private readonly ChatRepository _chatRepo;
        private readonly AccountRepository _accountRepo;

        public MessagesModel(ChatRepository chatRepository, AccountRepository accountRepo)
        {
            _chatRepo = chatRepository;
            _accountRepo = accountRepo;
        }

        public List<ChatMessage> Messages = new List<ChatMessage>();

        public async Task OnGet()
        {
            var currentUser = await _accountRepo.FindSiteUser(User.Identity.Name);
            if (currentUser != null)
            {
                Messages = GetLastMessage(await _chatRepo.GetRecentMessages(currentUser.Id));
            }                    
        }

        private List<ChatMessage> GetLastMessage(List<ChatMessage> messages)
        {
            var chatIds = messages.Select(e => e.ChatId).Distinct();

            var list  =  (from ch in chatIds
                          let msg = messages.First(e => e.ChatId == ch)
                          select msg);

            return list.OrderByDescending(e => e.CreatedOn).ToList();
        }

    }
}