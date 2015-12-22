namespace FinanceTracker.UniversalApp.UI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Control;
    using Data.Contracts;
    using Data.Exceptions;
    using Data.Models.Transactions;

    public class HomePageViewModel : BaseViewModel
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

        public double BalanceAmount { get; private set; }

        public int LastTransactionsCount { get; set; }

        public async void RefreshInformation()
        {
            var balanceInfo = await this.dataProvider.GetBalanceInformationAsync();
            this.BalanceAmount = balanceInfo.BalanceAmount;
            this.RaisePropertyChanged(nameof(this.BalanceAmount));

            try
            {
                KeyValuePair<string, string>[] parameters =
                {
                    new KeyValuePair<string, string>("page", "1"),
                    new KeyValuePair<string, string>("size", DefaultTransactionsCount.ToString())
                };

                var transactions = await this.dataProvider.GetTransactionsAsync(parameters);

                this.LastTransactions.Clear();
                foreach (var transaction in transactions)
                {
                    this.LastTransactions.Add(transaction);
                }
            }
            catch (ApplicationException e)
            {
                var messageDialog = new ErrorNotificationDialog();
                string[] messages = { e.Message };
                messageDialog.DataContext = messages;

                await messageDialog.ShowAsync();
            }
        }
    }
}