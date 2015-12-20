namespace FinanceTracker.WebApi.AspApi.Models
{
    using Data.DbModels;
    using Mappings;

    public class CategoryBindingModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}