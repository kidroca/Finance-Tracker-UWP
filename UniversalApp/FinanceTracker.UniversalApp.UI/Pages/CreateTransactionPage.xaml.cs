

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FinanceTracker.UniversalApp.UI.Pages
{
    using System;
    using Telerik.UI.Xaml.Controls.Input;
    using ViewModels;
    using Windows.ApplicationModel.Resources.Core;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateTransactionPage : Page
    {
        public CreateTransactionPage()
        {
            this.InitializeComponent();
            this.DataContext = new TransactionViewModel();
            InputLocalizationManager.Instance.UserResourceMap =
                ResourceManager.Current.MainResourceMap.GetSubtree("CustomHeaders");
            this.TransactionDatePicker.MinValue = new DateTime(1990, 1, 1);
            this.TransactionDatePicker.MaxValue = new DateTime(2090, 12, 31);
        }

        public TransactionViewModel TransactionViewModel
        {
            get { return this.DataContext as TransactionViewModel; }

            set { this.DataContext = value; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter == null) return;
        }
    }
}
