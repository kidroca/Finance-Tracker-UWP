namespace FinanceTracker.Data.DbModels.Transactions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Transaction
    {
        public int Id { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Transaction name should be at least 2 characters long")]
        [MaxLength(128, ErrorMessage = "Transaction name should be less than 128 characters")]
        public string Name { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Transaction amount should always be of positive value")]
        public decimal Amount { get; set; }

        [Required]
        public string BalanceId { get; set; }

        public virtual Balance Balance { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public DateTime DateTime { get; set; }

        [MinLength(2, ErrorMessage = "Transaction description should be at least 2 characters long")]
        [MaxLength(1000, ErrorMessage = "Transaction description should be no longer than 1000 characters")]
        public string Descriptions { get; set; }

        public TransactionType Type { get; set; }

        public bool IsDeleted { get; set; }
    }
}