using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FinanceTracker.UniversalApp.UI.Control
{
    using Data.Models.User;

    public sealed partial class RegistrationDialog : ContentDialog
    {
        public RegistrationDialog(UserRegisterModel dataContext)
        {
            this.InitializeComponent();
            this.DataContext = dataContext;
        }
    }
}
