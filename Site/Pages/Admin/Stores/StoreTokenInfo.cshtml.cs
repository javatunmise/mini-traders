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

namespace site.Pages.Admin.Stores
{
    public class StoreTokenInfoModel : PageModel
    {
        private readonly ApplicationDbContext context;

        public StoreTokenInfoModel(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Store CurrentStore { get; private set; }
        public TransactionAccount TokenAccount { get; private set; }
        public decimal TokenBalance { get; private set; }
        public List<TransactionEntry> TransHistories { get; private set; }

        public async Task<IActionResult> OnGet(int id)
        {
            CurrentStore = await context.Stores.AsNoTracking()
                                 .Include(x => x.User)
                                 .FirstOrDefaultAsync(x => x.Id == id);

            TokenAccount = context.TransactionAccounts.AsNoTracking().SingleOrDefault(e => e.AccountId == CurrentStore.User.TokenAccountCode);

            if (TokenAccount != null)
            {
                TransHistories = await context.TransactionEntries.AsNoTracking()
                                .Where(e => e.TransactionAccountId == TokenAccount.Id)
                                .OrderByDescending(e => e.CreatedOn)
                                .ToListAsync();

                TokenBalance = TransHistories.Sum(e => e.Amount);
            }

            return Page();
        }
    }
}
