namespace FinanceTracker.UniversalApp.UI.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    using Models.Transactions;

    public interface IData
    {
        Task<IEnumerable<TransactionModel>> GetTransactionsAsync(
            string category = null, int page = 1, int size = 10);

        Task<IEnumerable<string>> GetCategoriesAsync();

        Task<BalanceResponseModel> GetBalanceInformationAsync();

        Task<TransactionModel> AddTransactionAsync(TransactionModel transaction);

        Task AddCategoryAsync(string category);
    }
}