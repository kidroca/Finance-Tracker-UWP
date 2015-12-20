namespace FinanceTracker.UniversalApp.UI.Control
{
    using System;
    using Helpers;

    public class AppBarButtonContent : NotifyChangeModel
    {
        private bool isEnabled;

        public AppBarButtonContent()
        {
            this.IsEnabled = true;
        }

        public string Title { get; set; }

        public Type DestinationPageType { get; set; }

        public bool IsEnabled
        {
            get
            {
                return this.isEnabled;
            }

            set
            {
                this.isEnabled = value;
                this.RaisePropertyChanged(nameof(this.IsEnabled));
            }
        }
    }
}