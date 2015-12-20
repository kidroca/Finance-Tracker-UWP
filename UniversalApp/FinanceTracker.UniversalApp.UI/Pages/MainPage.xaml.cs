namespace FinanceTracker.UniversalApp.UI.Pages
{
    using Data;
    using ViewModels;
    using Windows.UI.Xaml.Controls;

    public sealed partial class MainPage : Page
    {
        public MainPage() : this(new RestApiData("http://localhost:61454"))
        {
        }

        public MainPage(IDataAuth dataProvider)
        {
            this.InitializeComponent();
            this.ViewModel = new MainPageViewModel(dataProvider);

        }

        public MainPageViewModel ViewModel
        {
            get { return this.DataContext as MainPageViewModel; }

            set { this.DataContext = value; }
        }
    }
}
