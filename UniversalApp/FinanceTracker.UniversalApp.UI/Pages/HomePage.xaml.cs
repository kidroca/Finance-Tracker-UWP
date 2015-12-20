namespace FinanceTracker.UniversalApp.UI.Pages
{
    using ViewModels;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
            this.DataContext = new HomePageViewModel();
        }

        public HomePageViewModel ViewModel
        {
            get { return this.DataContext as HomePageViewModel; }

            set { this.DataContext = value; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter == null) return;
        }

        private void OnAddTransactionClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CreateTransactionPage));
        }
    }
}
