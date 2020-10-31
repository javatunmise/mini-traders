using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Entities;
using site.Helpers;
using site.Repositories;
using Site.Data;

namespace site.Pages.Profile
{
    public class TransferTokenModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly AccountRepository _accountRepo;

        public TransferTokenModel(AccountRepository accountRepository, ApplicationDbContext context)
        {
            _dbContext = context;
            _accountRepo = accountRepository;
        }

        public decimal AccountBalance { get; private set; }

        public async Task<IActionResult> OnGet()
        {
            var currentUser = await _accountRepo.FindByUsername(User.Identity.Name);
            if (!currentUser.HasStore)
                return RedirectToPage("/Profile/Index");

            var siteUser = await _accountRepo.FindSiteUser(User.Identity.Name);
            SenderName = siteUser.FullName;

            AccountBalance = await GetAccountBalance(currentUser);

            return Page();
        }

        private async Task<decimal> GetAccountBalance(Account currentUser)
        {
            var account = await _dbContext.TransactionAccounts.AsNoTracking()
                                           .FirstOrDefaultAsync(e => e.SiteUserId == currentUser.Id && e.AccountType == Shared.Entities.AccountTypes.Token);
            if (account == null) return 0;


            return await _dbContext.TransactionEntries
                                    .Where(e => e.TransactionAccountId == account.Id)
                                    .SumAsync(e => e.Amount);
        }

        public async Task<IActionResult> OnPost()
        {
            var currentUser = await _accountRepo.FindByUsername(User.Identity.Name);
            AccountBalance = await GetAccountBalance(currentUser);

            var tokenTransferHandler = new TokenTransferHandler(_dbContext);
            var siteUser = await _accountRepo.FindSiteUser(User.Identity.Name);

            try
            {
                await tokenTransferHandler.Handle(siteUser, ReceipientWalletId, Amount.Value, SenderName);
                Success = true;
                return RedirectToPage();
            }
            catch(InvalidOperationException ioe)
            {
                ModelState.AddModelError("", ioe.Message);
            }

            return Page();
        }

        [BindProperty]
        [Required(ErrorMessage = "Your names are empty on your profile. Pls fill before continuing")]
        public string SenderName { get; set; }

        [BindProperty]
        [Required]
        public decimal? Amount { get; set; }

        [BindProperty]
        [Required]
        [Display(Name = "Vendor Token Id")]
        public string ReceipientWalletId { get; set; }

        [TempData]
        public bool Success { get; set; }
    }

    internal class TokenTransferHandler
    {
        private readonly ApplicationDbContext _dbContext;

        public TokenTransferHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        internal async Task Handle(SiteUser sender, string receipientWalletId, decimal transferAmount, string senderName = "")
        {
            senderName = string.IsNullOrWhiteSpace(senderName) ? sender.FullName : senderName;

            if (sender.WalletAccountCode == receipientWalletId) throw new InvalidOperationException("You cannot transfer to your own wallet");
            if (string.IsNullOrWhiteSpace(senderName)) throw new InvalidOperationException("Sender name cannot be empty");
            if (transferAmount <= 0) throw new InvalidOperationException("Invalid amount");

            var senderTokenAccount = await _dbContext.TransactionAccounts.SingleOrDefaultAsync(e => e.AccountId == sender.TokenAccountCode);
            if (senderTokenAccount == null) throw new InvalidOperationException("Invalid sender account");

            var balance = await _dbContext.TransactionEntries
                                    .Where(e => e.TransactionAccountId == senderTokenAccount.Id)
                                    .SumAsync(e => e.Amount);

            var receipientWalletAccount = await _dbContext.TransactionAccounts
                                                .Where(e => e.AccountId == receipientWalletId && e.AccountType == AccountTypes.Wallet)
                                                .Include(e => e.SiteUser)
                                                .SingleOrDefaultAsync();

            if (receipientWalletAccount == null) throw new InvalidOperationException("Invalid receipient wallet id");

            if (balance < transferAmount) throw new InvalidOperationException("Insufficient balance");

            var rcptAcc = receipientWalletAccount;

            var transEntries = new List<TransactionEntry>
            {
                new DebitEntry(transferAmount, senderTokenAccount.Id, $"Transfer to {rcptAcc.AccountId} ({rcptAcc.SiteUser.FullName})"),
                new CreditEntry(transferAmount, rcptAcc.Id, $"Transfer from {senderName}")
            };

            foreach (var tran in transEntries)
            {
                _dbContext.TransactionEntries.Add(tran);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
