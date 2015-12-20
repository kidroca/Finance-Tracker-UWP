// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FinanceTracker.UniversalApp.UI.Control
{
    using Windows.UI.Xaml.Controls;

    public sealed partial class ErrorNotificationDialog : ContentDialog
    {
        public ErrorNotificationDialog()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
