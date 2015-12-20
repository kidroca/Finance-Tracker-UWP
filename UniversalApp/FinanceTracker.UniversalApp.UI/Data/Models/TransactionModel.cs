namespace FinanceTracker.UniversalApp.UI.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using ViewModels;

    public class TransactionModel
    {
        [Display(Name = "Date")]
        public DateTime DateTime { get; set; }

        [Range(
            0,
            double.MaxValue,
            ErrorMessage = "Transaction value must be positive, use transaction type 'Widthdraw' to specify a 'negative' transaction"
            )]
        public decimal Amount { get; set; }

        public TransactionType Type { get; set; }

        public string Notes { get; set; }

        public int Id { get; set; }

        [Required(ErrorMessage = "Specify a transaction category")]
        public string Category { get; set; }
    }
}