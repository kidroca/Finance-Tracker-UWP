namespace FinanceTracker.UniversalApp.UI.Data.Models
{
    using System.Collections.Generic;

    public class TransactionsCollection
    {
        public IEnumerable<TransactionModel> Result { get; set; }
    }
}