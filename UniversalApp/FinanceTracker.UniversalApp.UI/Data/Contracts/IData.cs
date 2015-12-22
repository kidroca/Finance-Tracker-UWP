namespace FinanceTracker.UniversalApp.UI.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    using Models.Transactions;

    public interface IData
    {
        Task<IEnumerable<TransactionModel>> GetTransactionsAsync(
            params KeyValuePair<string, string>[] queryParameters);

        Task<IEnumerable<string>> GetCategoriesAsync();

        Task<BalanceResponseModel> GetBalanceInformationAsync();

        Task AddTransactionAsync(TransactionModel transaction);

        Task UpdateTransactionAsync(TransactionType transaction);
    }
}