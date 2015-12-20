namespace FinanceTracker.Data.DbModels.Transactions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Transaction
    {
        public int Id { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Transaction amount should always be of positive value")]
        public decimal Amount { get; set; }

        public string BalanceId { get; set; }

        public virtual Balance Balance { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public DateTime DateTime { get; set; }

        public TransactionType Type { get; set; }

        public bool IsDeleted { get; set; }
    }
}