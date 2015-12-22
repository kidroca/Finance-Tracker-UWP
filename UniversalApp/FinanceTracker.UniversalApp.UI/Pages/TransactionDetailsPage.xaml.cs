using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FinanceTracker.UniversalApp.UI.Pages
{
    using Data.Models.Transactions;
    using ViewModels;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TransactionDetailsPage : Page
    {
        public TransactionDetailsPage()
        {
            this.InitializeComponent();
            this.DataContext = new TransactionDetailsViewModel();
        }

        public TransactionDetailsViewModel ViewModel
        {
            get { return this.DataContext as TransactionDetailsViewModel; }

            set { this.DataContext = value; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var transaction = e.Parameter as TransactionModel;
            if (transaction == null) return;

            this.ViewModel.RefreshContent(transaction);
        }
    }
}
