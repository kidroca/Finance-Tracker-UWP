namespace FinanceTracker.UniversalApp.UI.Data
{
    using System.Threading.Tasks;
    using Models.User;

    public interface IDataAuth
    {
        Task<string> AuthenticateAsync(UserLoginModel user);

        Task RegisterAsync(UserRegisterModel user);

        string AuthenticationToken { set; }
    }
}