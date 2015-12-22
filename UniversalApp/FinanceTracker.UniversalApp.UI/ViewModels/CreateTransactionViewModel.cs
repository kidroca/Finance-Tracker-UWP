namespace FinanceTracker.UniversalApp.UI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Control;
    using Data.Contracts;
    using Data.Exceptions;
    using Data.Models.Transactions;
    using Helpers;
    using Pages;
    using Windows.UI.Popups;
    using Windows.UI.Xaml;

    public class CreateTransactionViewModel : BaseViewModel
    {
        private readonly IData dataProvider;

        private bool isAddButtonAvailable;

        public CreateTransactionViewModel(IData dataProvider)
        {
            this.AddTransactionCommand = new DelegateCommand<TransactionModel>(
                this.AddTransaction, this.IsButtonAvailable);
            this.CancelCommand = new DelegateCommand<object>(this.GoToPreviousPage);

            this.Categories = new ObservableCollection<string>();
            this.isAddButtonAvailable = true;

            this.dataProvider = dataProvider;
            this.RefreshCategories();
        }




        public static IEnumerable<TransactionType> TransactionTypes
        {
            get { return Enum.GetValues(typeof(TransactionType)).Cast<TransactionType>(); }
        }

        public ICommand AddTransactionCommand { get; }

        public ICommand CancelCommand { get; }

        public ObservableCollection<string> Categories { get; }

        public bool InProgress
        {
            get { return !this.isAddButtonAvailable; }
        }

        private async void AddTransaction(TransactionModel transactionData)
        {
            if (!this.IsValidModel(transactionData))
            {
                return;
            }

            this.isAddButtonAvailable = false;
            this.RaisePropertyChanged(nameof(this.AddTransactionCommand));
            this.RaisePropertyChanged(nameof(this.InProgress));

            try
            {
                // Todo: Waiting wheel animation
                await this.dataProvider.AddTransactionAsync(transactionData);

                var content = (Window.Current.Content as AppShell).AppFrame;
                content.Navigate(typeof(HomePage));
                var notification = new MessageDialog("Transaction added successfully!");
                await notification.ShowAsync();
            }
            catch (ApplicationException e)
            {
                // Notify User. Show Login Screen
                var messageDialog = new ErrorNotificationDialog();
                string[] messages = { e.Message };
                messageDialog.DataContext = messages;

                await messageDialog.ShowAsync();
            }
            finally
            {
                this.isAddButtonAvailable = true;
                this.RaisePropertyChanged(nameof(this.AddTransactionCommand));
                this.RaisePropertyChanged(nameof(this.InProgress));
            }
        }

        private async void RefreshCategories()
        {
            try
            {
                var categories = await this.dataProvider.GetCategoriesAsync();

                this.Categories.Clear();
                foreach (var category in categories)
                {
                    this.Categories.Add(category);
                }
            }
            catch (ApplicationException e)
            {
                var messageDialog = new ErrorNotificationDialog();
                string[] messages = { e.Message };
                messageDialog.DataContext = messages;

                await messageDialog.ShowAsync();
            }
        }

        private void GoToPreviousPage(object obj)
        {
            var page = obj as CreateTransactionPage;

            page?.Frame.GoBack();
        }

        private bool IsButtonAvailable(object obj)
        {
            return this.isAddButtonAvailable;
        }
    }
}