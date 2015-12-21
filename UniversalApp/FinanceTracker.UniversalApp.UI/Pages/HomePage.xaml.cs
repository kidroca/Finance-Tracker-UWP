namespace FinanceTracker.UniversalApp.UI.Pages
{
    using Data;
    using Data.Contracts;
    using ViewModels;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    public sealed partial class HomePage : Page
    {
        public HomePage() : this(RestApiData.GetInstance(baseUrl: "http://localhost:61454"))
        {
        }

        public HomePage(IData dataProvider)
        {
            this.InitializeComponent();
            this.DataContext = new HomePageViewModel(dataProvider);
        }

        public HomePageViewModel ViewModel
        {
            get { return this.DataContext as HomePageViewModel; }

            set { this.DataContext = value; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.ViewModel.RefreshInformation();
        }

        private void OnAddTransactionClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CreateTransactionPage));
        }
    }
}
