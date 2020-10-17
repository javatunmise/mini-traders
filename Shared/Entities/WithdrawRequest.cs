using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Entities
{
    public class WithdrawRequest
    {
        public WithdrawRequest()
        {
            WorkflowHistories = new HashSet<WithdrawRequestWorkflowHistory>();
        }
        public int Id { get; set; }
        public int TransactionAccountId { get; set; }
        public string WalletCode { get; set; }
        public int SiteUserId { get; set; }
        public SiteUser SiteUser { get; set; }
        public decimal Amount { get; set; }
        public WithdrawRequestStatuses Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public ICollection<WithdrawRequestWorkflowHistory> WorkflowHistories { get; set; }
        public decimal Charge { get; set; }
    }
    public class WithdrawRequestWorkflowHistory
    {
        public int Id { get; set; }
        public WithdrawRequest Request { get; set; }
        public WithdrawRequestStatuses Status { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public enum WithdrawRequestStatuses { Submitted, Cancelled, Processed, Declined }
}
