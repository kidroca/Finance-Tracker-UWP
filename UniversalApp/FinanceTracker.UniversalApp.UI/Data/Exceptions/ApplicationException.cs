namespace FinanceTracker.UniversalApp.UI.Data.Exceptions
{
    using System;

    public class ApplicationException : Exception
    {
        public ApplicationException(string message) : base(message)
        {
        }
    }
}