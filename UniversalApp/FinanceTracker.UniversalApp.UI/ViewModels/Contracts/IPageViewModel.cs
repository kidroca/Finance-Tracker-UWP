namespace FinanceTracker.UniversalApp.UI.ViewModels.Contracts
{
    public interface IPageViewModel
    {
        string Title { get; }

        IContentViewModel ContentViewModel { get; }
    }
}