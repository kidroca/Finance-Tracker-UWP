namespace FinanceTracker.UniversalApp.UI.Data.Models.User
{
    using System.ComponentModel.DataAnnotations;

    public class UserRegisterModel : UserLoginModel
    {
        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}