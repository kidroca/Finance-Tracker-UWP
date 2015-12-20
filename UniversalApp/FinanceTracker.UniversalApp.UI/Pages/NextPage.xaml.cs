namespace FinanceTracker.UniversalApp.UI.Pages
{
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    public sealed partial class NextPage : Page
    {
        public NextPage()
        {
            this.InitializeComponent();
            //this.navBar.NavItems = new[]
            //{
            //    new AppBarButtonContent
            //    {
            //        Title = "Main",
            //        DestinationPageType = typeof(MainPage)
            //    }
            //};
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter == null)
            {
                return;
            }

            this.tbDate.Text = e.Parameter.ToString();
        }
    }
}
