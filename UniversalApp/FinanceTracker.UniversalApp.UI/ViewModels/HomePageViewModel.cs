namespace FinanceTracker.UniversalApp.UI.ViewModels
{
    using Data.Contracts;
    using Helpers;

    public class HomePageViewModel : NotifyChangeModel
    {
        private IData dataProvider;

        private decimal currentAmount;

        public HomePageViewModel(IData dataProvider)
        {
            this.dataProvider = dataProvider;
            this.Balance = new BalanceViewModel();
        }

        public BalanceViewModel Balance { get; set; }

        public async void RefreshBalance()
        {
            var balanceInfo = await this.dataProvider.GetBalanceInformationAsync();
            this.Balance.CurrentAmount = balanceInfo.BalanceAmount;
        }
    }
}