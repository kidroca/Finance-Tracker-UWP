namespace FinanceTracker.UniversalApp.UI.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.CompilerServices;
    using Annotations;
    using Control;

    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool IsValidModel<T>(T model)
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