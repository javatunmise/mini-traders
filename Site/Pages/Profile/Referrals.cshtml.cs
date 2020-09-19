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
    public class ReferralsModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly AccountRepository _accountRepo;

        public ReferralsModel(ApplicationDbContext context, AccountRepository accountRepository)
        {
            _dbContext = context;
            _accountRepo = accountRepository;
        }

        public List<Referral> Referrals { get; private set; } = new List<Referral>();

        public async Task<IActionResult> OnGet()
        {
            var account = await _accountRepo.FindByUsername(User.Identity.Name);
            if (!account.HasStore)
                return RedirectToPage("/Profile/Index");

            var currentUser = await _dbContext.SiteUsers.FirstOrDefaultAsync(e => e.Email == User.Identity.Name);
            if (currentUser != null)
            {
                Referrals = await _dbContext.Referrals.Where(e => e.ReferrerUserId == currentUser.Id)
                                                      .Include(e => e.Store).ToListAsync();
                ReferralCode = currentUser.ReferralCode;
            }

            return Page();
        }

        public string ReferralCode { get; set; }
    }
}