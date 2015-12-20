namespace FinanceTracker.WebApi.AspApi.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using AutoMapper.QueryableExtensions;
    using Data.DbModels;
    using Data.Handlers.Repositories;
    using Models;

    [Authorize]
    public class CategoriesController : Common
    {
        private readonly IRepository<User> usersRepo;
        private readonly IRepository<Category> categoriesRepo;

        public CategoriesController(
            IRepository<User> usersRepository,
            IRepository<Category> categoriesRepository)
        {
            this.usersRepo = usersRepository;
            this.categoriesRepo = categoriesRepository;
        }

        public IHttpActionResult Get()
        {
            var categories = this.categoriesRepo.All()
                .ProjectTo<CategoryBindingModel>()
                .ToList();

            var result = this.Ok(categories);

            return result;
        }

        public IHttpActionResult Post(string category)
        {
            if (string.IsNullOrEmpty(category.Trim()) || category.Length < 2)
            {
                return this.BadRequest("Invalid category name");
            }

            var user = this.usersRepo.GetById(this.UserId);
            category = base.FormatCategoryName(category);

            var dbCategory = this.categoriesRepo.All()
                .FirstOrDefault(c => c.Name == category);

            if (dbCategory == null)
            {
                this.categoriesRepo.Add(new Category
                {
                    Name = category
                }).SaveChanges();

                return this.Created("api/Categories/{0}", dbCategory.Id);
            }
            else
            {
                return this.BadRequest("This category already exist");
            }
        }

        public IHttpActionResult Delete(string category, bool revertTransactions = true)
        {
            category = base.FormatCategoryName(category);

            var user = this.usersRepo.GetById(this.UserId);
            var dbCategory = this.categoriesRepo.All().FirstOrDefault(c => c.Name == category);

            if (dbCategory == null)
            {
                return this.NotFound();
            }
            else
            {
                string redirect = this.Url.Link("DefaultApi", new { controller = "Transactions" });
                redirect += this.Request.RequestUri.Query;

                return this.Redirect(redirect);
            }
        }
    }
}
