using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using Site.Data;

namespace site.Pages.Profile
{
    public class ReferralsModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public ReferralsModel(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public List<Referral> Referrals { get; private set; } = new List<Referral>();

        public async Task OnGet()
        {
            var currentUser = await _dbContext.SiteUsers.FirstOrDefaultAsync(e => e.Email == User.Identity.Name);
            if (currentUser != null)
            {
                Referrals = await _dbContext.Referrals.Where(e => e.ReferrerUserId == currentUser.Id)
                                                      .Include(e => e.Store).ToListAsync();
                ReferralCode = currentUser.ReferralCode;
            }                    
        }

        public string ReferralCode { get; set; }
    }
}