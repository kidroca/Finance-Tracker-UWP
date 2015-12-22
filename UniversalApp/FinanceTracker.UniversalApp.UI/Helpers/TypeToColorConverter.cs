namespace FinanceTracker.UniversalApp.UI.Helpers
{
    using System;
    using ViewModels;
    using Windows.UI;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Media;
    using Data.Models.Transactions;

    public class TypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (targetType != typeof(Brush))
            {
                return null;
            }

            TransactionType type;
            if (!Enum.TryParse(value.ToString(), out type))
            {
                return null;
            }

            if (type == TransactionType.Deposit)
            {
                return new SolidColorBrush(Colors.LawnGreen);
            }
            else
            {
                return new SolidColorBrush(Colors.DarkOrange);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}