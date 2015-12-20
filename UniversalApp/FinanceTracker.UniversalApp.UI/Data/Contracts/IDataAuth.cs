namespace FinanceTracker.UniversalApp.UI.Data.Contracts
{
    using System.Threading.Tasks;
    using Models.User;

    public interface IDataAuth
    {
        Task<string> AuthenticateAsync(UserLoginModel user);

        Task RegisterAsync(UserRegisterModel user);

        bool IsAuthenticated { get; }

        string AuthenticationToken { set; }
    }
}