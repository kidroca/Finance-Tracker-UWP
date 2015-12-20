using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace FinanceTracker.UniversalApp.UI.Control
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Pages;

    public sealed partial class NavigationControl : UserControl
    {
        public NavigationControl()
        {
            this.InitializeComponent();
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var destination = (btn.DataContext as AppBarButtonContent).DestinationPageType;

            if (this.CurrentPage == destination)
            {
                return;
            }

            var content = (Window.Current.Content as AppShell).AppFrame;
            content.Navigate(destination);

            this.CurrentPage = destination;
        }

        public static readonly DependencyProperty NavItemsProperty = DependencyProperty.Register(
            "NavItems",
            typeof(IEnumerable<AppBarButtonContent>),
            typeof(NavigationControl),
            new PropertyMetadata(null, HandleNavItemsChanged));

        public Type CurrentPage { get; private set; }

        public IEnumerable<AppBarButtonContent> NavItems
        {
            get { return (IEnumerable<AppBarButtonContent>)GetValue(NavItemsProperty); }
            set { SetValue(NavItemsProperty, value); }
        }

        private static void HandleNavItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var navView = d as NavigationControl;
            if (navView == null) return;

            var buttons = e.NewValue as IEnumerable<AppBarButtonContent>;
            if (buttons == null) return;

            Task.Run(() =>
            {
                foreach (var button in buttons)
                {
                    if (button.DestinationPageType == navView.CurrentPage)
                    {
                        button.IsEnabled = false;
                    }
                }
            });
        }
    }
}
