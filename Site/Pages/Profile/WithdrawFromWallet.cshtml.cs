using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shared;
using Shared.Entities;
using site.Data;
using site.Repositories;
using Site.Data;

namespace site.Pages.Profile
{
    public class WithdrawFromWalletModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly AccountRepository _accountRepo;
        private readonly IConfiguration _configuration;
        private readonly ICurrentDate _serverDate;
        private readonly ISiteContentProvider _provider;

        public WithdrawFromWalletModel(AccountRepository accountRepository, 
                        ApplicationDbContext context, 
                        IConfiguration configuration,
                        ICurrentDate serverDate, 
                        ISiteContentProvider siteContentProvider)
        {
            _dbContext = context;
            _accountRepo = accountRepository;
            _configuration = configuration;
            _serverDate = serverDate;
            _provider = siteContentProvider;
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

            var site = await _provider.GetCurrentSite();
            WithdrawalCharge = site == null ? 50 : site.WithdrawalCharge;

            return Page();
        }

        private async Task<decimal> GetAccountBalance(Account currentUser)
        {
            var account = await _dbContext.TransactionAccounts.AsNoTracking()
                                           .FirstOrDefaultAsync(e => e.SiteUserId == currentUser.Id && e.AccountType == Shared.Entities.AccountTypes.Wallet);
            if (account == null) return 0;


            return await _dbContext.TransactionEntries
                                    .Where(e => e.TransactionAccountId == account.Id)
                                    .SumAsync(e => e.Amount);
        }

        public async Task<IActionResult> OnPost()
        {
            var currentUser = await _accountRepo.FindByUsername(User.Identity.Name);
            AccountBalance = await GetAccountBalance(currentUser);

            var siteUser = await _accountRepo.FindSiteUser(User.Identity.Name);

            var site = await _provider.GetCurrentSite();
            WithdrawalCharge = site == null ? 50 : site.WithdrawalCharge;

            try
            {
                var siteOWnerAccountCode = _configuration["SiteOwnerAccountCode"];
                var tokenTransferHandler = new SubmitWithrawalRequestHandler(_dbContext, _serverDate);
                await tokenTransferHandler.Handle(siteUser, siteOWnerAccountCode, Amount.Value, WithdrawalCharge, SenderName);
                Success = true;
            }
            catch (InvalidOperationException ioe)
            {
                ModelState.AddModelError("", ioe.Message);
            }

            return RedirectToPage("/Profile/WithdrawRequests");
        }

        [BindProperty]
        [Required(ErrorMessage = "Please complete your profile details: missing first name or last name")]
        public string SenderName { get; set; }

        [BindProperty]
        [Required]
        public decimal? Amount { get; set; }

        public bool Success { get; private set; }
        public decimal WithdrawalCharge { get; private set; }
    }

    internal class SubmitWithrawalRequestHandler
    {
        private readonly ApplicationDbContext _dbContext;
        private ICurrentDate serverDate;

        public SubmitWithrawalRequestHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public SubmitWithrawalRequestHandler(ApplicationDbContext dbContext, ICurrentDate serverDate) : this(dbContext)
        {
            this.serverDate = serverDate;
        }

        internal async Task Handle(SiteUser siteUser, string siteOwnerAccountCode, decimal withdrawAmount, decimal charge, string senderName = "")
        {

            senderName = string.IsNullOrWhiteSpace(senderName) ? siteUser.FullName : senderName;
            if (string.IsNullOrWhiteSpace(senderName)) throw new InvalidOperationException("Sender name cannot be empty");

            if (withdrawAmount <= 0) throw new InvalidOperationException("Invalid amount");

            var userWalletAccount = await _dbContext.TransactionAccounts.SingleOrDefaultAsync(e => e.AccountId == siteUser.WalletAccountCode);
            if (userWalletAccount == null) throw new InvalidOperationException("Invalid sender account");

            var balance = await _dbContext.TransactionEntries
                                   .Where(e => e.TransactionAccountId == userWalletAccount.Id)
                                   .SumAsync(e => e.Amount);

            var totalAmount = withdrawAmount + charge;
            if (balance < totalAmount) throw new InvalidOperationException($"Insufficient balance: you need a total of N{totalAmount} in your wallet");

            var siteOwnerAccount = await _dbContext.TransactionAccounts
                                                .Where(e => e.AccountId == siteOwnerAccountCode && e.AccountType == AccountTypes.SiteOwner)
                                                .SingleOrDefaultAsync();

            if (siteOwnerAccount == null) throw new InvalidOperationException("Missing site owner account. Please contact site administrator.");

            var rcptAcc = siteOwnerAccount;

            var transEntries = new List<TransactionEntry>
            {
                new DebitEntry(totalAmount, userWalletAccount.Id, "Withdrawal from Wallet"),
                new CreditEntry(totalAmount, siteOwnerAccount.Id, $"Withdraw from Wallet: {siteUser.WalletAccountCode} ({senderName})")
            };

            foreach (var tran in transEntries)
            {
                _dbContext.TransactionEntries.Add(tran);
            }

            var wdr = new WithdrawRequest
            {
                CreatedOn = serverDate.Now(),
                SiteUserId = siteUser.Id,
                Status = WithdrawRequestStatuses.Submitted,
                Amount = withdrawAmount,
                Charge = charge,
                TransactionAccountId = userWalletAccount.Id,
                WalletCode = userWalletAccount.AccountId
            };

            wdr.WorkflowHistories.Add(new WithdrawRequestWorkflowHistory
            {
                CreatedOn = serverDate.Now(),
                Status = WithdrawRequestStatuses.Submitted
            });

            _dbContext.WithdrawRequests.Add(wdr);

            await _dbContext.SaveChangesAsync();
        }
    }

}
