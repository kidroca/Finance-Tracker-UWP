namespace FinanceTracker.UniversalApp.UI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Windows.Input;
    using Control;
    using Data.Contracts;
    using Data.Exceptions;
    using Data.Models.User;
    using Helpers;
    using Pages;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public class MainPageViewModel : NotifyChangeModel
    {
        private readonly IDataAuth dataProvider;
        private bool isLoginAvailable;

        public MainPageViewModel(IDataAuth dataProvider)
        {
            this.dataProvider = dataProvider;
            this.isLoginAvailable = true;

            this.LoginCommand = new DelegateCommand<UserLoginModel>(this.LogIn, this.IsLogginAvailable);
            this.RegisterCommand = new DelegateCommand<Panel>(this.Register);
        }
        public ICommand LoginCommand { get; }

        public ICommand RegisterCommand { get; }

        private async void Register(Panel container)
        {
            var regModel = new UserRegisterModel();
            var registrationForm = new RegistrationDialog(regModel);
            container.Children.Add(registrationForm);

            var result = await registrationForm.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                if (this.IsValidModel(regModel))
                {
                    try
                    {
                        await this.dataProvider.RegisterAsync(regModel);
                    }
                    catch (ApplicationException e)
                    {
                        string[] messages = { e.Message };
                        var errorDialog = new ErrorNotificationDialog();
                        errorDialog.DataContext = messages;
                        await errorDialog.ShowAsync();
                    }

                    this.LogIn(regModel);
                }
            }
        }

        private async void LogIn(UserLoginModel userLoginModel)
        {
            if (!this.IsValidModel(userLoginModel))
            {
                return;
            }

            this.isLoginAvailable = false;
            this.RaisePropertyChanged(nameof(this.LoginCommand));

            try
            {
                // Waiting wheel animation for the login
                string token = await this.dataProvider.AuthenticateAsync(userLoginModel);
                this.dataProvider.AuthenticationToken = token;

                var content = (Window.Current.Content as AppShell).AppFrame;
                content.Navigate(typeof(HomePage));
            }
            catch (ApplicationException e)
            {
                // Notify User. Show Login Screen
                var messageDialog = new ErrorNotificationDialog();
                string[] messages = { "Wrong username or password, try again" };
                messageDialog.DataContext = messages;

                await messageDialog.ShowAsync();
            }
            finally
            {
                this.isLoginAvailable = true;
                this.RaisePropertyChanged(nameof(this.LoginCommand));
            }
        }

        private bool IsLogginAvailable(object obj)
        {
            return this.isLoginAvailable;
        }

        private bool IsValidModel<T>(T model)
        {
            var context = new ValidationContext(model, serviceProvider: null, items: null);
            var violations = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(model, context, violations, validateAllProperties: true);

            if (!isValid)
            {
                var dialog = new ErrorNotificationDialog { DataContext = violations };
                dialog.ShowAsync();
            }

            return isValid;
        }
    }
}