namespace FinanceTracker.UniversalApp.UI.Pages
{
    using Control;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public sealed partial class AppShell : Page
    {
        private static readonly AppBarButtonContent[] navItems = {
                new AppBarButtonContent
                {
                    Title = "Next",
                    DestinationPageType = typeof(NextPage)
                },
                new AppBarButtonContent
                {
                    Title = "Home",
                    DestinationPageType = typeof(MainPage)
                }
            };

        public AppShell()
        {
            this.InitializeComponent();
            this.navBar.NavItems = navItems;
        }

        public Frame AppFrame
        {
            get { return this.ContentFrame; }
        }

        private void OnButtonBackPress(object sender, RoutedEventArgs e)
        {
            if (this.AppFrame.CanGoBack)
            {
                this.AppFrame.GoBack();
            }
        }
    }
}
