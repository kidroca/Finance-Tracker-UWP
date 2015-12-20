namespace FinanceTracker.Data.DbModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Transactions;

    public class Category
    {
        private ICollection<Transaction> transactions;

        public Category()
        {
            this.transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [MinLength(2, ErrorMessage = "Category must be at least 2 characters long")]
        [MaxLength(128)]
        public string Name { get; set; }

        public virtual ICollection<Transaction> Transactions
        {
            get { return this.transactions; }

            set { this.transactions = value; }
        }
    }
}