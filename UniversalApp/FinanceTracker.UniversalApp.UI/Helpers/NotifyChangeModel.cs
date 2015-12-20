namespace FinanceTracker.UniversalApp.UI.Helpers
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Annotations;

    public abstract class NotifyChangeModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}