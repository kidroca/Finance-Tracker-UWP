namespace FinanceTracker.UniversalApp.UI.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Helpers;

    public class BalanceViewModel : NotifyChangeModel
    {
        private decimal currentAmount;
        private ObservableCollection<CategoryViewModel> categories;

        public decimal CurrentAmount
        {
            get
            {
                return this.currentAmount;
            }

            set
            {
                this.currentAmount = value;
                this.RaisePropertyChanged(nameof(this.CurrentAmount));
            }
        }

        public ObservableCollection<TransactionViewModel> Transactions { get; set; }

        public ObservableCollection<CategoryViewModel> Categories
        {
            get
            {
                if (this.categories == null)
                {
                    this.categories = new ObservableCollection<CategoryViewModel>();
                }

                return this.categories;
            }

            set
            {
                this.categories.Clear();
                foreach (var model in value)
                {
                    this.categories.Add(model);
                }
            }
        }

        public async Task<CategoryViewModel> CreateCategory(CategoryViewModel newCategory)
        {
            // Todo: Push to server and get Id

            await Task.Run(() =>
            {
                this.Categories.Add(newCategory);
            });

            return newCategory;
        }

        public async Task<TransactionViewModel> CreateTransaction(TransactionViewModel newTransaction)
        {
            // Todo: Push to server and get Id

            await Task.Run(() =>
            {
                this.Transactions.Add(newTransaction);
            });

            return newTransaction;
        }
    }
}