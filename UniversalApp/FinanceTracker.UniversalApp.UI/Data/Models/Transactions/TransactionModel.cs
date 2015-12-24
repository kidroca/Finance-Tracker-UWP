namespace FinanceTracker.UniversalApp.UI.Data.Models.Transactions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    public class TransactionModel
    {
        public TransactionModel()
        {
            // Sets the initial selected date to the current for convenience
            this.DateTime = DateTime.Now;
        }

        [Display(Name = "Date")]
        public DateTime DateTime { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Transaction name should be at least 2 characters long")]
        [MaxLength(128, ErrorMessage = "Transaction name should be less than 128 characters")]
        public string Name { get; set; }

        [Range(
            0,
            double.MaxValue,
            ErrorMessage = "Transaction value must be positive, use transaction type 'Widthdraw' to specify a 'negative' transaction"
            )]
        public double Amount { get; set; }

        public TransactionType Type { get; set; }

        [MinLength(2, ErrorMessage = "Transaction notes should be at least 2 characters long")]
        [MaxLength(1000, ErrorMessage = "Transaction notes should be no longer than 1000 characters")]
        [JsonProperty("description")]
        public string Notes { get; set; }

        public int Id { get; set; }

        [Required(ErrorMessage = "Specify a transaction category")]
        public string Category { get; set; }
    }
}