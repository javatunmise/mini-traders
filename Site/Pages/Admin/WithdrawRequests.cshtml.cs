using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shared;
using Shared.Entities;
using site.Repositories;
using Site.Data;

namespace site.Pages.Admin
{
    public class WithdrawRequestsModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly AccountRepository _accountRepo;
        private readonly ICurrentDate _serverDate;
        private readonly IConfiguration _configuration;

        public WithdrawRequestsModel(ApplicationDbContext dbContext, AccountRepository accountRepository, 
                                    ICurrentDate serverDate, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _accountRepo = accountRepository;
            _serverDate = serverDate;
            _configuration = configuration;
        }

        public List<WithdrawRequest> Requests { get; private set; }

        public async Task OnGet()
        {
            var currentUser = await _accountRepo.FindByUsername(User.Identity.Name);

            Requests = await GetWithdrawalRequests();
        }

        public async Task<IActionResult> OnPost()
        {
            var currentUser = await _accountRepo.FindByUsername(User.Identity.Name);

            Requests = await GetWithdrawalRequests();

            var request = await _dbContext.WithdrawRequests.Include(e => e.WorkflowHistories).SingleOrDefaultAsync(e => e.Id == RequestId);
            if (request != null && IsValidForCancelling(request, currentUser))
            {
                var siteOwnerAccountCode = _configuration["SiteOwnerAccountCode"];

                await new CancelWithdrawalRequestHandler(_dbContext, _serverDate).Handle(request, siteOwnerAccountCode);
            }

            return RedirectToPage();
        }

        private bool IsValidForCancelling(WithdrawRequest request, Shared.Account currentUser)
        {
            return request.Status == WithdrawRequestStatuses.Submitted && request.SiteUserId == currentUser.Id;
        }

        private Task<List<WithdrawRequest>> GetWithdrawalRequests()
        {
            return _dbContext.WithdrawRequests
                                   .Where(e => e.Status == Status)
                                   .Include(e => e.WorkflowHistories)
                                   .OrderByDescending(e => e.CreatedOn)
                                   .ToListAsync();
        }

        [BindProperty]
        public int RequestId { get; set; }

        [FromQuery]
        public WithdrawRequestStatuses Status { get; set; }
    }

    internal class CancelWithdrawalRequestHandler
    {
        private ApplicationDbContext _dbContext;
        private ICurrentDate _serverDate;

        public CancelWithdrawalRequestHandler(ApplicationDbContext dbContext, ICurrentDate serverDate)
        {
            _dbContext = dbContext;
            _serverDate = serverDate;
        }

        internal async Task Handle(WithdrawRequest request, string siteOwnerAccountCode)
        {
            SiteUser siteUser = request.SiteUser;
            TransactionAccount userWalletAccount = await GetUserWalletAccount(siteUser);
            if (userWalletAccount == null) throw new InvalidOperationException("Invalid sender account");
            TransactionAccount siteOwnerAccount = await GetSiteOwnerAccount(siteOwnerAccountCode);
            if (siteOwnerAccount == null) throw new InvalidOperationException("Missing site owner account. Please contact site administrator.");

            CancelRequest(request);

            var totalAmount = request.Charge + request.Amount;

            var transEntries = new List<TransactionEntry>
            {
                new CreditEntry(totalAmount, userWalletAccount.Id, "Reversal from cancellation of withdrawal request"),
                new DebitEntry(totalAmount, siteOwnerAccount.Id, $"User cancelled withdraw request: {siteUser.WalletAccountCode} ({siteUser.FullName})")
            };

            foreach (var tran in transEntries)
            {
                _dbContext.TransactionEntries.Add(tran);
            }

            await _dbContext.SaveChangesAsync();
        }

        private void CancelRequest(WithdrawRequest request)
        {
            request.Status = WithdrawRequestStatuses.Cancelled;
            request.WorkflowHistories?.Add(new WithdrawRequestWorkflowHistory
            {
                CreatedOn = _serverDate.Now(),
                Status = WithdrawRequestStatuses.Cancelled
            });
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