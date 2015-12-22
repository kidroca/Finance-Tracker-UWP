namespace FinanceTracker.UniversalApp.UI.Data.Models.Transactions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TransactionModel
    {
        public TransactionModel()
        {
            // Sets the initial selected date to the current for convenience
            this.DateTime = DateTime.Now;
        }

        [Display(Name = "Date")]
        public DateTime DateTime { get; set; }

        [Range(
            0,
            double.MaxValue,
            ErrorMessage = "Transaction value must be positive, use transaction type 'Widthdraw' to specify a 'negative' transaction"
            )]
        public double Amount { get; set; }

        public TransactionType Type { get; set; }

        public string Notes { get; set; }

        public int Id { get; set; }

        [Required(ErrorMessage = "Specify a transaction category")]
        public string Category { get; set; }
    }
}