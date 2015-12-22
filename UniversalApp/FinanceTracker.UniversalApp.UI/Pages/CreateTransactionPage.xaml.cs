

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FinanceTracker.UniversalApp.UI.Pages
{
    using System;
    using Data;
    using Data.Contracts;
    using Telerik.UI.Xaml.Controls.Input;
    using ViewModels;
    using Windows.ApplicationModel.Resources.Core;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateTransactionPage : Page
    {
        public CreateTransactionPage() : this(RestApiData.GetInstance(baseUrl: "http://localhost:61454"))
        {
        }

        public CreateTransactionPage(IData provider)
        {
            this.InitializeComponent();
            this.DataContext = new CreateTransactionViewModel(provider);

            InputLocalizationManager.Instance.UserResourceMap =
                ResourceManager.Current.MainResourceMap.GetSubtree("CustomHeaders");
            this.TransactionDatePicker.MinValue = new DateTime(1990, 1, 1);
            this.TransactionDatePicker.MaxValue = new DateTime(2090, 12, 31);
        }

        public CreateTransactionViewModel CreateTransactionViewModel
        {
            get { return this.DataContext as CreateTransactionViewModel; }

            set { this.DataContext = value; }
        }
    }
}
