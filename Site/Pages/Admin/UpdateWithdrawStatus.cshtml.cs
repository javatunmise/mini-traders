using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shared;
using Shared.Entities;
using Site.Data;

namespace site.Pages.Admin
{
    public class UpdateWithdrawStatusModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly ICurrentDate _serverDate;

        public UpdateWithdrawStatusModel(ApplicationDbContext dbContext,
                                         IConfiguration configuration,
                                         ICurrentDate serverDate)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _serverDate = serverDate;
        }

        public WithdrawRequest CurrentRequest { get; private set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null) return RedirectToPage("/Error404");

            CurrentRequest = await _dbContext.WithdrawRequests.Include(e => e.SiteUser).AsNoTracking()
                                   .FirstOrDefaultAsync(e => e.Id == id.Value);

            if (CurrentRequest == null) return RedirectToPage("/Error404");

            ShowForm = CurrentRequest.Status == WithdrawRequestStatuses.Submitted;

            return Page();
        }
        public async Task<IActionResult> OnPost(int? id)
        {
            if (id == null) return RedirectToPage("/Error404");

            CurrentRequest = await _dbContext.WithdrawRequests.Include(e => e.SiteUser)
                                   .FirstOrDefaultAsync(e => e.Id == id.Value);
            if (CurrentRequest == null) return RedirectToPage("/Error404");

            if (CurrentRequest.Status == WithdrawRequestStatuses.Submitted)
            {
                if (Status == WithdrawRequestStatuses.Declined)
                {
                    var siteOwnerAccountCode = _configuration["SiteOwnerAccountCode"];
                    await new CancelWithdrawalRequestHandler(_dbContext, _serverDate).Handle(CurrentRequest, siteOwnerAccountCode);
                }

                if (Status == WithdrawRequestStatuses.Processed)
                {
                    CurrentRequest.Status = WithdrawRequestStatuses.Processed;
                    CurrentRequest.WorkflowHistories?.Add(new WithdrawRequestWorkflowHistory
                    {
                        CreatedOn = _serverDate.Now(),
                        Status = WithdrawRequestStatuses.Processed
                    });

                    await _dbContext.SaveChangesAsync();
                }
            }

            return RedirectToPage("/Admin/WithdrawRequests");
        }

        [BindProperty]
        public WithdrawRequestStatuses Status { get; set; }
        public bool ShowForm { get; private set; }
    }

    class DeclineWithdrawalRequestHandler
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICurrentDate _serverDate;

        public DeclineWithdrawalRequestHandler(ApplicationDbContext dbContext, ICurrentDate serverDate)
        {
            _dbContext = dbContext;
            _serverDate = serverDate;
        }
        internal async Task Handle(WithdrawRequest request, string siteOwnerAccountCode, decimal withdrawAmount, decimal charge, string senderName = "")
        {
            SiteUser siteUser = request.SiteUser;
            TransactionAccount userWalletAccount = await GetUserWalletAccount(siteUser);
            if (userWalletAccount == null) throw new InvalidOperationException("Invalid sender account");
            TransactionAccount siteOwnerAccount = await GetSiteOwnerAccount(siteOwnerAccountCode);
            if (siteOwnerAccount == null) throw new InvalidOperationException("Missing site owner account. Please contact site administrator.");

            request.Status = WithdrawRequestStatuses.Declined;
            request.WorkflowHistories?.Add(new WithdrawRequestWorkflowHistory
            {
                CreatedOn = _serverDate.Now(),
                Status = WithdrawRequestStatuses.Declined
            });

            var totalAmount = request.Charge + request.Amount;

            var transEntries = new List<TransactionEntry>
            {
                new CreditEntry(totalAmount, userWalletAccount.Id, "Reversal from decline of withdrawal request"),
                new DebitEntry(totalAmount, siteOwnerAccount.Id, $"Admin declined withdraw request: {siteUser.WalletAccountCode} ({siteUser.FullName})")
            };

            foreach (var tran in transEntries)
            {
                _dbContext.TransactionEntries.Add(tran);
            }

            await _dbContext.SaveChangesAsync();
        }
        private async Task<TransactionAccount> GetUserWalletAccount(SiteUser siteUser)
        {
            return await _dbContext.TransactionAccounts.SingleOrDefaultAsync(e => e.AccountId == siteUser.WalletAccountCode);
        }

        private async Task<TransactionAccount> GetSiteOwnerAccount(string siteOwnerAccountCode)
        {
            return await _dbContext.TransactionAccounts
                                   .SingleOrDefaultAsync(e => e.AccountId == siteOwnerAccountCode && e.AccountType == AccountTypes.SiteOwner);
        }
    }
}