namespace FinanceTracker.Data.DatabaseLink
{
    using System.Data.Entity;
    using DbModels;
    using DbModels.Transactions;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class FinanceTrackerDbContext : IdentityDbContext<User>
    {
        public FinanceTrackerDbContext() : base("FinanceTracker", throwIfV1Schema: false)
        {
        }

        // public IDbSet<ApplicationUser> ApplicationUsers { get; set; }

        public IDbSet<Balance> Balances { get; set; }

        public IDbSet<Transaction> Transactions { get; set; }

        public IDbSet<Category> Categories { get; set; }

        public static FinanceTrackerDbContext Create()
        {
            return new FinanceTrackerDbContext();
        }
    }
}
