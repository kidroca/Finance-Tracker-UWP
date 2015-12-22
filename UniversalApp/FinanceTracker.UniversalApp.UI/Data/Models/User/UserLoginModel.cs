namespace FinanceTracker.UniversalApp.UI.Data.Models.User
{
    using System.ComponentModel.DataAnnotations;

    public class UserLoginModel
    {
        public const string PasswordErrorMessage = "Password should be at least 6 characters long";

        /// <summary>
        /// For Testing convenience
        /// </summary>
        public UserLoginModel()
        {
        }

        [Required]
        [MinLength(6, ErrorMessage = PasswordErrorMessage)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Username { get; set; }
    }
}