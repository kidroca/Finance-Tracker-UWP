namespace FinanceTracker.UniversalApp.UI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Helpers;

    public class TransactionViewModel : NotifyChangeModel
    {
        private decimal amount;

        public static IEnumerable<TransactionType> TransactionTypes
        {
            get { return Enum.GetValues(typeof(TransactionType)).Cast<TransactionType>(); }
        }

        public string[] Categories
        {
            get
            {
                return new string[]
                {
                    "Pesho", "Gosho", "Misho"
                };
            }
        }

        [Display(Name = "Date")]
        public DateTime DateTime { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Transaction value must be positive, use transaction type 'Widthdraw' to specify a 'negative' transaction")]
        public decimal Amount
        {
            get
            {
                // Testing
                if (this.amount <= 0)
                {
                    return 10;
                }

                return this.amount;
            }

            set
            {
                Validator.ValidateProperty(value, new ValidationContext(this)
                {
                    MemberName = nameof(this.Amount)
                });

                this.amount = value;
            }
        }

        public TransactionType Type { get; set; }

        public string Notes { get; set; }

        // public int Id { get; private set; }

        public string Category { get; set; }
    }

    public enum TransactionType
    {
        Deposit = 0,
        Withdraw = 1
    }
}