namespace FinanceTracker.Data.DbModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Transactions;

    public class Balance
    {
        private ICollection<Transaction> transactions;

        public Balance()
        {
            this.transactions = new HashSet<Transaction>();
        }

        [Key]
        [ForeignKey("User")]
        public string BalanceId { get; set; }

        public decimal CurrentAmount { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Transaction> Transactions
        {
            get { return this.transactions; }

            set { this.transactions = value; }
        }
    }
}