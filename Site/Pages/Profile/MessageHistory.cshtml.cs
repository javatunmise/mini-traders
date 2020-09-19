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
    public class MessageHistoryModel : PageModel
    {
        private readonly ChatRepository _chatRepo;
        private readonly AccountRepository _accountRepo;

        public MessageHistoryModel(ChatRepository chatRepository, AccountRepository accountRepo)
        {
            _chatRepo = chatRepository;
            _accountRepo = accountRepo;
        }

        public List<ChatMessage> Messages = new List<ChatMessage>();
        public string RecipientName { get; set; } 

        public async Task OnGet(int id)
        {
            var currentUser = await _accountRepo.FindSiteUser(User.Identity.Name);
            if (currentUser != null)
            {
                Messages = (await _chatRepo.GetChatMessages(id, currentUser.Id));

                if (Messages.Any())
                {
                    var chat = Messages.First();
                    RecipientName = $"{chat.SiteUser?.FirstName} {chat.SiteUser?.LastName}";
                }
            }
        }

    }
}