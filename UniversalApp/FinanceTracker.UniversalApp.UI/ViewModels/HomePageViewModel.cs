namespace FinanceTracker.UniversalApp.UI.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using Data.Contracts;
    using Data.Models.Transactions;
    using Helpers;

    public class HomePageViewModel : NotifyChangeModel
    {
        public const int DefaultTransactionsCount = 10;

        private readonly IData dataProvider;

        public HomePageViewModel(
            IData dataProvider, int lastTransactionsCount = DefaultTransactionsCount)
        {
            this.dataProvider = dataProvider;
            this.LastTransactionsCount = lastTransactionsCount;
            this.LastTransactions = new ObservableCollection<TransactionModel>();
        }

        public ObservableCollection<TransactionModel> LastTransactions { get; private set; }

        public decimal BalanceAmount { get; private set; }

        public int LastTransactionsCount { get; set; }

        public async void RefreshInformation()
        {
            var balanceInfo = await this.dataProvider.GetBalanceInformationAsync();
            this.BalanceAmount = balanceInfo.BalanceAmount;
            this.RaisePropertyChanged(nameof(this.BalanceAmount));

            this.LastTransactions.Add(new TransactionModel
            {
                DateTime = DateTime.Today,
                Amount = 100,
                Category = "TestDeposit",
                Type = TransactionType.Deposit,
            });

            this.LastTransactions.Add(new TransactionModel
            {
                DateTime = DateTime.Today,
                Amount = 50,
                Category = "TestWithdraw",
                Type = TransactionType.Withdraw,
            });

            this.LastTransactions.Add(new TransactionModel
            {
                DateTime = DateTime.Today,
                Amount = 14,
                Category = "TestDeposit",
                Type = TransactionType.Deposit,
            });
        }
    }
}