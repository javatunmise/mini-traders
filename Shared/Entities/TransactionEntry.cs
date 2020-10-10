using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Entities
{
    public class TransactionEntry
    {
        public int Id { get; set; }
        public TransactionAccount Account { get; set; }
        public int TransactionAccountId { get; set; }
        public decimal Amount { get; set; }
        public string Narration { get; set; }
        public DateTime CreatedOn { get; set; }
        public TransactionEntryTypes EntryType { get; set; }
    }

    public class CreditEntry : TransactionEntry
    {
        public CreditEntry(decimal tokenAmount, int accountId, string narration)
        {
            Amount = tokenAmount;
            TransactionAccountId = accountId;
            EntryType = TransactionEntryTypes.Credit;
            CreatedOn = System.DateTime.Now;
            Narration = narration;
        }
    }

    public class DebitEntry : TransactionEntry
    {
        public DebitEntry(decimal tokenAmount, int accountId, string narration)
        {
            Amount = -1 * tokenAmount;
            TransactionAccountId = accountId;
            EntryType = TransactionEntryTypes.Debit;
            CreatedOn = System.DateTime.Now;
            Narration = narration;
        }
    }
}

namespace Shared
{
    public enum TransactionEntryTypes
    {
        Credit = 1,
        Debit = 2
    }
}