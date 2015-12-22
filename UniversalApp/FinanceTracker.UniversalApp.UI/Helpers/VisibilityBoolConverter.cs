namespace FinanceTracker.UniversalApp.UI.Helpers
{
    using System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    public class VisibilityBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (targetType != typeof(Visibility)) return null;

            if (value.GetType() != typeof(bool)) return null;

            bool visibility = (bool)value;

            return visibility ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (targetType != typeof(bool)) return null;

            Visibility visibility;
            if (!Enum.TryParse(value.ToString(), out visibility))
            {
                return null;
            }

            return visibility == Visibility.Visible;
        }
    }
}