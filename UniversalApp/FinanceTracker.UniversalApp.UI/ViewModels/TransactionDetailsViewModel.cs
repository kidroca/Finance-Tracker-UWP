namespace FinanceTracker.UniversalApp.UI.ViewModels
{
    using System.Reflection;
    using Data.Models.Transactions;

    public class TransactionDetailsViewModel : BaseViewModel
    {
        public TransactionDetailsViewModel()
        {
            this.Transaction = new TransactionModel();
        }

        public TransactionModel Transaction { get; set; }

        public void RefreshContent(TransactionModel transaction)
        {
            foreach (var property in transaction.GetType().GetProperties())
            {
                var value = property.GetValue(transaction);
                property.SetValue(this.Transaction, value);
            }

            this.RaisePropertyChanged(nameof(this.Transaction));
        }
    }
}