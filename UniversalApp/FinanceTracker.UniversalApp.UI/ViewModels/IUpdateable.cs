namespace FinanceTracker.UniversalApp.UI.ViewModels
{
    using System.Threading.Tasks;

    public interface IUpdateable
    {
        Task UpdateValuesAsync();
    }
}