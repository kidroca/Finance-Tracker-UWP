namespace FinanceTracker.UniversalApp.UI.ViewModels
{
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;

    public class CategoryViewModel
    {
        private string name;

        public int Id { get; private set; }

        [Required]
        [MinLength(2, ErrorMessage = "Category must be at least 2 symbols")]
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                Validator.ValidateProperty(value, new ValidationContext(this)
                {
                    MemberName = nameof(this.Name)
                });

                this.name = value;
            }
        }

        public ObservableCollection<CreateTransactionViewModel> Transactions { get; set; }
    }
}